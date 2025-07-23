using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{

    public class PlayerService : IPlayerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PlayerService> _logger;
        private readonly IConfiguration _configuration;

        // Poprawny konstruktor
        public PlayerService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<PlayerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task SetGameProcessPos(int gameId, int teamId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Pobierz NOWĄ instancję DbContext z tego zakresu
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Krok 1: Oblicz sumy i od razu połącz je z GameProcessId.
                var calculatedPositions = await context.GameProcesses
                    .Where(gp => gp.GameId == gameId && gp.TeamId == teamId)
                    .Join(
                        context.GameLogSpecs.Where(gls => gls.GameLog.GameId == gameId && gls.GameLog.TeamId == teamId && gls.GameProcessId != null),
                        gameProcess => gameProcess.ProcessId, // Klucz z GameProcess
                        gameLogSpec => gameLogSpec.GameProcessId,   // Klucz z GameLogSpec
                        (gameProcess, gameLogSpec) => new { gameProcess, gameLogSpec } // Wynik połączenia
                    )
                    .GroupBy(joined => joined.gameProcess.GameProcessId) // Grupujemy po docelowym ID
                    .Select(group => new
                    {
                        GameProcessId = group.Key,
                        FinalPosX = group.Sum(g => g.gameLogSpec.MoveX),
                        FinalPosY = group.Sum(g => g.gameLogSpec.MoveY)
                    })
                    .ToDictionaryAsync(p => p.GameProcessId);

                // Krok 2: Pobierz wpisy GameBoard do aktualizacji.
                var gameBoardEntriesToUpdate = await context.GameBoards
                    .Where(gb => gb.GameId == gameId && gb.TeamId == teamId && gb.GameProcessId != null)
                    .ToListAsync();

                // Krok 3: Zaktualizuj pozycje.
                foreach (var entry in gameBoardEntriesToUpdate)
                {
                    if (entry.GameProcessId.HasValue && calculatedPositions.TryGetValue(entry.GameProcessId.Value, out var newPosition))
                    {
                        entry.PozX = newPosition.FinalPosX / 100;
                        entry.PozY = newPosition.FinalPosY / 100;
                    }
                }

                // Krok 4: Zapisz zmiany.
                await context.SaveChangesAsync();
            }
            await SetTeamPos(gameId, teamId);

        }
        public async Task SetTeamPos(int gameId, int teamId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Pobierz NOWĄ instancję DbContext z tego zakresu
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                // Krok 1: Pobierz w jednym zapytaniu wszystkie niezbędne dane:
                // pozycje pionków-procesów oraz wagi tych procesów.
                var processPawns = await context.GameBoards
                    .Where(gb =>
                        gb.GameId == gameId &&
                        gb.TeamId == teamId &&
                        gb.GameProcessId != null) // Pobieramy tylko pionki-procesy
                    .Select(gb => new
                    {
                        PosX = gb.PozX,
                        PosY = gb.PozY,
                        Weight = gb.GameProcess.Process.ProcessWeight // Pobieramy wagę z powiązanego procesu
                    })
                    .ToListAsync();

                if (!processPawns.Any())
                {
                    // _logger.LogWarning("Nie znaleziono pionków-procesów dla drużyny {TeamId} w grze {GameId}. Nie można obliczyć średniej.", teamId, gameId);
                    return; // Jeśli nie ma pionków, nie ma czego obliczać.
                }

                // Krok 2: Oblicz średnią ważoną w pamięci.
                // Formuła: Średnia ważona = Σ(pozycja * waga) / Σ(waga)

                double totalWeight = processPawns.Sum(p => p.Weight);

                // Zabezpieczenie przed dzieleniem przez zero, jeśli wszystkie wagi są zerowe.
                if (totalWeight == 0)
                {
                    // _logger.LogWarning("Suma wag procesów dla drużyny {TeamId} w grze {GameId} wynosi zero. Ustawiam pozycję na (0,0).", teamId, gameId);
                    // W takim przypadku można ustawić pozycję na (0,0) lub na zwykłą średnią arytmetyczną.
                    // Dla bezpieczeństwa ustawmy na 0.
                    totalWeight = 1; // Uniknięcie dzielenia przez zero, wynik będzie 0.
                }

                double weightedSumX = processPawns.Sum(p => p.PosX * p.Weight);
                double weightedSumY = processPawns.Sum(p => p.PosY * p.Weight);

                int finalAvgX = (int)Math.Round(weightedSumX / totalWeight);
                int finalAvgY = (int)Math.Round(weightedSumY / totalWeight);

                // Krok 3: Znajdź główny pionek drużyny (ten z GameProcessId = null).
                var teamPawnEntry = await context.GameBoards
                    .FirstOrDefaultAsync(gb =>
                        gb.GameId == gameId &&
                        gb.TeamId == teamId &&
                        gb.GameProcessId == null); // Kluczowy filtr: główny pionek

                if (teamPawnEntry == null)
                {
                    // _logger.LogError("BŁĄD KRYTYCZNY: Nie znaleziono głównego pionka (GameProcessId is null) dla drużyny {TeamId} w grze {GameId}.", teamId, gameId);
                    return;
                }

                // Krok 4: Zaktualizuj pozycję głównego pionka.
                teamPawnEntry.PozX = finalAvgX;
                teamPawnEntry.PozY = finalAvgY;

                // Krok 5: Zapisz zmiany w bazie danych.
                await context.SaveChangesAsync();

                // _logger.LogInformation("Zaktualizowano średnią ważoną pozycję ({PosX}, {PosY}) dla drużyny {TeamId} w grze {GameId}.", finalAvgX, finalAvgY, teamId, gameId);
            }
        }

    }
}