using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services
{
    public interface IPlayerService
    {
        Task SetGameProcessPosAsync(int gameId, int teamId);
        Task SetTeamPosAsync(int gameId, int teamId);
    }
    /// Serwis odpowiedzialny za logikę związaną z pozycją graczy i drużyn na planszy.
    public class PlayerService : IPlayerService
    {
        // Zależność wstrzykiwana bezpośrednio
        private readonly AppDbContext _context;
        private readonly ILogger<PlayerService> _logger;
        // IConfiguration pozostawione na wypadek przyszłych potrzeb, np. odczytu stałych z pliku konfiguracyjnego
        private readonly IConfiguration _configuration;

        // Stała zamiast "magicznej liczby"
        private const int PositionDivisor = 100;

        private readonly IHubContext<GameHub> _hubContext;

        public PlayerService(AppDbContext context, IConfiguration configuration, ILogger<PlayerService> logger, IHubContext<GameHub> hubContext)
        {
            // Używanie strażników (guards) do walidacji argumentów
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        /// Aktualizuje pozycje pionków-procesów na podstawie logów gry.
        public async Task SetGameProcessPosAsync(int gameId, int teamId)
        {
            int currentGameId = gameId;
            int currentTeamId = teamId;

            _logger.LogInformation("Rozpoczynanie aktualizacji pozycji dla GameId: {GameId}, TeamId: {TeamId}", currentGameId, currentTeamId);

            // =================================================================================
            // KROK 1: Oblicz sumy ruchów, grupując po OGÓLNYM ProcessId.
            // Zaczynamy od GameLogSpecs, bo tam są ruchy.
            // Używamy nawigacji, aby dostać się do ProcessId z powiązanego GameProcess.
            // =================================================================================
            var movesByGeneralProcessId = await _context.GameLogSpecs
                .AsNoTracking() // Używamy AsNoTracking, bo tylko czytamy dane do agregacji
                .Where(gls =>
                    gls.GameLog.GameId == currentGameId &&
                    gls.GameLog.TeamId == currentTeamId &&
                    gls.GameProcessId.HasValue &&       // Upewniamy się, że GameProcess istnieje
                    gls.GameProcess.ProcessId != null) // Upewniamy się, że powiązany Process istnieje
                .GroupBy(gls => gls.GameProcess.ProcessId) // KLUCZOWY MOMENT: Grupujemy po ProcessId z tabeli nadrzędnej!
                .Select(group => new
                {
                    ProcessId = group.Key, // Kluczem jest teraz ProcessId, tak jak chciałeś.
                    FinalPosX = group.Sum(g => g.MoveX),
                    FinalPosY = group.Sum(g => g.MoveY)
                })
                .ToDictionaryAsync(p => p.ProcessId, p => new { p.FinalPosX, p.FinalPosY });

            if (!movesByGeneralProcessId.Any())
            {
                _logger.LogInformation("Brak nowych ruchów do przetworzenia.");
                return;
            }

            _logger.LogInformation("Obliczono pozycje dla {Count} typów procesów.", movesByGeneralProcessId.Count);

            // =================================================================================
            // KROK 2: Zaktualizuj wszystkie pionki na planszy, które pasują do obliczonych ProcessId.
            // Tym razem pobieramy encje do aktualizacji, więc NIE używamy AsNoTracking.
            // Musimy dołączyć (Include) powiązany GameProcess, aby mieć dostęp do ProcessId.
            // =================================================================================
            var gameBoardEntriesToUpdate = await _context.GameBoards
                .Include(gb => gb.GameProcess) // Dołączamy GameProcess, aby mieć dostęp do ProcessId
                .Where(gb =>
                    gb.GameId == currentGameId &&
                    gb.TeamId == currentTeamId &&
                    gb.GameProcessId != null &&
                    gb.GameProcess.ProcessId != null)
                .ToListAsync();

            _logger.LogInformation("Znaleziono {Count} pionków-procesów na planszy do potencjalnej aktualizacji.", gameBoardEntriesToUpdate.Count);

            foreach (var entry in gameBoardEntriesToUpdate)
            {
                // Sprawdzamy, czy dla ProcessId danego pionka mamy obliczoną nową pozycję.
                if (movesByGeneralProcessId.TryGetValue(entry.GameProcess.ProcessId, out var newPosition))
                {
                    var finalX = newPosition.FinalPosX / PositionDivisor;
                    var finalY = newPosition.FinalPosY / PositionDivisor;

                    _logger.LogInformation(
                        "Aktualizacja pionka GameBoardId: {GameBoardId} (z ProcessId: {ProcessId}). Nowa pozycja: ({X}, {Y})",
                        entry.GameBoardId,
                        entry.GameProcess.ProcessId,
                        finalX,
                        finalY);

                    entry.PozX = finalX;
                    entry.PozY = finalY;
                }
            }

            // Krok 3: Zapisz wszystkie zmiany.
            await _context.SaveChangesAsync();
            _logger.LogInformation("Zakończono aktualizację pozycji.");
            
            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("BoardUpdated");
        }

        /// Aktualizuje pozycję głównego pionka drużyny jako średnią ważoną pozycji pionków-procesów.
        public async Task SetTeamPosAsync(int gameId, int teamId)
        {
            // Krok 1: Pobierz dane o pionkach-procesach (pozycja i waga) w jednym zapytaniu.
            var processPawns = await _context.GameBoards
                .Where(gb => gb.GameId == gameId && gb.TeamId == teamId && gb.GameProcessId != null)
                .Select(gb => new
                {
                    gb.PozX,
                    gb.PozY,
                    Weight = gb.GameProcess.Process.ProcessWeight
                })
                .ToListAsync();

            if (!processPawns.Any())
            {
                _logger.LogWarning("Nie znaleziono pionków-procesów dla drużyny {TeamId} w grze {GameId}. Nie można obliczyć średniej pozycji drużyny.", teamId, gameId);
                return;
            }

            // Krok 2: Oblicz średnią ważoną w pamięci.
            double totalWeight = processPawns.Sum(p => p.Weight);
            if (totalWeight == 0)
            {
                _logger.LogWarning("Suma wag procesów dla drużyny {TeamId} w grze {GameId} wynosi zero. Ustawiam pozycję na (0,0).", teamId, gameId);
                // Aby uniknąć dzielenia przez zero, można przyjąć domyślną pozycję lub zwrócić błąd.
                // Ustawienie na (0,0) jest bezpiecznym wyjściem.
                totalWeight = 1;
            }

            double weightedSumX = processPawns.Sum(p => p.PozX * p.Weight);
            double weightedSumY = processPawns.Sum(p => p.PozY * p.Weight);

            int finalAvgX = (int)Math.Round(weightedSumX / totalWeight);
            int finalAvgY = (int)Math.Round(weightedSumY / totalWeight);

            // Krok 3: Znajdź i zaktualizuj główny pionek drużyny.
            var teamPawnEntry = await _context.GameBoards
                .FirstOrDefaultAsync(gb => gb.GameId == gameId && gb.TeamId == teamId && gb.GameProcessId == null);

            if (teamPawnEntry == null)
            {
                _logger.LogError("BŁĄD KRYTYCZNY: Nie znaleziono głównego pionka (GameProcessId is null) dla drużyny {TeamId} w grze {GameId}.", teamId, gameId);
                return;
            }

            teamPawnEntry.PozX = finalAvgX;
            teamPawnEntry.PozY = finalAvgY;

            // Krok 4: Zapisz zmiany.
            await _context.SaveChangesAsync();
            _logger.LogInformation("Zaktualizowano średnią ważoną pozycję ({PosX}, {PosY}) dla drużyny {TeamId} w grze {GameId}.", finalAvgX, finalAvgY, teamId, gameId);

            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("BoardUpdated");
            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("BoardUpdated", new { teamId = teamId });
        }
    }
}