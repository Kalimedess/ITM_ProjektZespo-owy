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
    // Krok 1: Pobierz wszystkie pionki-procesy dla drużyny, łącznie z ich pozycjami i wagami.
    // Teraz pobieramy również ProcessId, aby móc śledzić, które procesy mają wagę.
    var processPawns = await _context.GameBoards
        .Where(gb => gb.GameId == gameId && gb.TeamId == teamId && gb.GameProcessId != null)
        .Select(gb => new PawnData
        {
            ProcessId = gb.GameProcess.ProcessId, // Pobieramy ID ogólnego procesu
            PosX = gb.PozX,
            PosY = gb.PozY,
            Weight = gb.GameProcess.Process.ProcessWeight
        })
        .ToListAsync();

    if (!processPawns.Any())
    {
        _logger.LogWarning("Nie znaleziono pionków-procesów dla drużyny {TeamId} w grze {GameId}. Nie można obliczyć pozycji.", teamId, gameId);
        return;
    }

    // Krok 2: Przetwórz wagi - zidentyfikuj brakujące i dokonaj redystrybucji.
    List<NormalizedPawnData> normalizedPawns = ProcessAndNormalizeWeights(processPawns);

    // Krok 3: Oblicz średnią ważoną na podstawie znormalizowanych wag.
    // Ponieważ suma znormalizowanych wag wynosi 1, nie musimy przez nią dzielić.
    double weightedSumX = normalizedPawns.Sum(p => p.PosX * p.NormalizedWeight);
    double weightedSumY = normalizedPawns.Sum(p => p.PosY * p.NormalizedWeight);

    // Wynik nie wymaga już dzielenia, bo suma wag to 1.
    int finalAvgX = (int)Math.Round(weightedSumX);
    int finalAvgY = (int)Math.Round(weightedSumY);

    // Krok 4: Znajdź i zaktualizuj główny pionek drużyny (logika pozostaje taka sama).
    var teamPawnEntry = await _context.GameBoards
        .FirstOrDefaultAsync(gb => gb.GameId == gameId && gb.TeamId == teamId && gb.GameProcessId == null);

    if (teamPawnEntry == null)
    {
        _logger.LogError("BŁĄD KRYTYCZNY: Nie znaleziono głównego pionka dla drużyny {TeamId} w grze {GameId}.", teamId, gameId);
        return;
    }

    teamPawnEntry.PozX = finalAvgX;
    teamPawnEntry.PozY = finalAvgY;

    // Krok 5: Zapisz zmiany i wyślij powiadomienie.
    await _context.SaveChangesAsync();
    _logger.LogInformation("Zaktualizowano średnią ważoną pozycję ({PosX}, {PosY}) dla drużyny {TeamId} w grze {GameId}.", finalAvgX, finalAvgY, teamId, gameId);

    // Powiadomienie SignalR (zakładając, że _hubContext jest wstrzyknięty)
    await _hubContext.Clients.Group(gameId.ToString()).SendAsync("BoardUpdated", new { teamId });
}

/// <summary>
/// Prywatna metoda pomocnicza do przetwarzania i normalizacji wag.
/// To jest serce nowej logiki biznesowej.
/// </summary>
private List<NormalizedPawnData> ProcessAndNormalizeWeights(List<PawnData> pawns)
{
    // Dzielimy pionki na te z wagą i te bez (waga <= 0)
    var pawnsWithWeight = pawns.Where(p => p.Weight > 0).ToList();
    var pawnsWithoutWeight = pawns.Where(p => p.Weight <= 0).ToList();

    double totalWeightProvided = pawnsWithWeight.Sum(p => p.Weight);
    double missingWeight = 1.0 - totalWeightProvided; // Zakładamy, że docelowa suma to 1.0

    // Jeśli wagi są poprawne (suma >= 1) lub nie ma komu oddać brakującej wagi,
    // po prostu je znormalizuj i zwróć.
    if (missingWeight <= 0 || !pawnsWithWeight.Any())
    {
        _logger.LogInformation("Wagi nie wymagają redystrybucji. Normalizowanie {PawnCount} pionków.", pawns.Count);
        return Normalize(pawns, pawns.Sum(p => p.Weight));
    }

    _logger.LogInformation(
        "Wykryto brakującą wagę: {MissingWeight}. Rozdzielanie jej między {PawnCount} pionków.",
        missingWeight, pawnsWithWeight.Count);
        
    var temporaryWeights = new Dictionary<int, double>();

    // Przypisz oryginalne wagi
    foreach (var pawn in pawnsWithWeight)
    {
        temporaryWeights[pawn.ProcessId] = pawn.Weight;
    }

    // Dokonaj redystrybucji brakującej wagi proporcjonalnie
    foreach (var pawn in pawnsWithWeight)
    {
        // Udział danego pionka w sumie istniejących wag
        double proportion = pawn.Weight / totalWeightProvided;
        // Dodaj do jego wagi odpowiednią część brakującej wagi
        temporaryWeights[pawn.ProcessId] += missingWeight * proportion;
    }

    // Teraz stwórz finalną listę z nowymi, redystrybuowanymi wagami.
    var finalPawnData = pawns.Select(p => new PawnData
    {
        ProcessId = p.ProcessId,
        PosX = p.PosX,
        PosY = p.PosY,
        // Użyj nowej wagi, jeśli istnieje, w przeciwnym razie waga to 0
        Weight = temporaryWeights.GetValueOrDefault(p.ProcessId, 0)
    }).ToList();

    // Na koniec znormalizuj wszystko, aby mieć pewność, że suma wynosi dokładnie 1.
    return Normalize(finalPawnData, finalPawnData.Sum(p => p.Weight));
}

/// <summary>
/// Normalizuje listę wag tak, aby ich suma wynosiła 1.0.
/// </summary>
private List<NormalizedPawnData> Normalize(List<PawnData> pawns, double totalWeight)
{
    // Zabezpieczenie przed dzieleniem przez zero
    if (totalWeight == 0)
    {
        _logger.LogWarning("Całkowita suma wag wynosi 0, nie można znormalizować. Zwracam wagi zerowe.");
        return pawns.Select(p => new NormalizedPawnData(p, 0)).ToList();
    }
    
    return pawns.Select(p => new NormalizedPawnData(p, p.Weight / totalWeight)).ToList();
}

// --- Klasy pomocnicze DTO do przechowywania danych w pamięci ---

/// <summary>
/// Przechowuje surowe dane o pionku pobrane z bazy.
/// </summary>
private class PawnData
{
    public int ProcessId { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public double Weight { get; set; }
}

/// <summary>
/// Przechowuje dane o pionku wraz ze znormalizowaną wagą.
/// </summary>
private class NormalizedPawnData
{
    public int ProcessId { get; }
    public double PosX { get; }
    public double PosY { get; }
    public double NormalizedWeight { get; }

    public NormalizedPawnData(PawnData source, double normalizedWeight)
    {
        ProcessId = source.ProcessId;
        PosX = source.PosX;
        PosY = source.PosY;
        NormalizedWeight = normalizedWeight;
    }
}
    }
}