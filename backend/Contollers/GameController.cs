using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


public class ProcessDto
{
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}
public class TeamDto
{
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
    public bool IsAbleToMakeDecisions { get; set; }
    
}
public class CreateGameDto
{
    public string GameName { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public int RivalBoardId { get; set; }
    public int DeckId { get; set; }
    public bool GameMode { get; set; }
    public int NumberOfTeams { get; set; }
    public int StartBits { get; set; }
    public List<TeamDto> Teams { get; set; } = new List<TeamDto>();
    public List<ProcessDto> Processes { get; set; } = new List<ProcessDto>();
}

public class GameListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}


namespace backend.Controllers
{

    [Route("api/games")]
    [ApiController]
    public class GamesViewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesViewController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Nieprawidłowy identyfikator użytkownika do utworzenia gry.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return Unauthorized("Użytkownik nie został znaleziony.");
            }

            if (user.LicensesOwned <= 0)
            {
                return BadRequest(new { message = "Brak dostępnych licencji do utworzenia nowej gry." });
            }

            // Pobierz wszystkie potrzebne procesy Z BAZY DANYCH za jednym razem, aby uniknąć zapytań w pętli (problem N+1).
            // Używamy słownika dla błyskawicznego dostępu w pętli.
            var requestedProcessShortNames = gameDto.Processes.Select(p => p.ShortName).ToList();
            var processesFromDb = await _context.Processes
                .Where(p => p.DeckId == gameDto.DeckId && requestedProcessShortNames.Contains(p.ProcessDesc))
                .ToDictionaryAsync(p => p.ProcessDesc);

            // Sprawdź, czy wszystkie żądane procesy istnieją w podanej talii
            if (processesFromDb.Count != requestedProcessShortNames.Count)
            {
                return BadRequest(new { message = "Jeden lub więcej wybranych procesów nie istnieje w podanej talii kart." });
            }


            // --- 2. Użycie transakcji dla zapewnienia integralności danych ---
            await using var transaction = await _context.Database.BeginTransactionAsync();

           try
        {
            // --- 3. Tworzenie encji głównych ---
            var newGame = new Game
            {
                GameDesc = gameDto.GameName,
                TeamBoardId = gameDto.BoardId,
                RivalBoardId = gameDto.RivalBoardId,
                DeckId = gameDto.DeckId,
                GameStatus = GameStatus.During,
                UserId = userId,
                IsOnline = gameDto.GameMode
            };
            _context.Games.Add(newGame);

            // Przechowamy listę nowo utworzonych drużyn, aby później utworzyć dla nich GameBoards
            var newTeams = new List<Team>();

            foreach (var teamDto in gameDto.Teams)
            {
                var gameTeam = new Team
                {
                    TeamName = teamDto.Name,
                    TeamColor = teamDto.Colour,
                    TeamBud = gameDto.StartBits,
                    TeamToken = TokenGenerator.GenerateRandomAlphanumericToken(6),
                    IsIndependent = teamDto.IsAbleToMakeDecisions,
                    Game = newGame // Powiązanie z grą (EF Core sam ustawi GameId)
                };

                // Dla każdej drużyny utwórz powiązania z wybranymi procesami
                foreach (var processDto in gameDto.Processes)
                {
                    // Pobierz wcześniej załadowany proces ze słownika
                    var dbProcess = processesFromDb[processDto.ShortName];

                    var newGameProcess = new GameProcess
                    {
                        Game = newGame,
                        Team = gameTeam,
                        ProcessId = dbProcess.ProcessId // KLUCZOWY MOMENT: przypisanie ID procesu z bazy
                    };
                    // EF Core doda to do kontekstu, gdy dodamy gameTeam do newGame.Teams
                    gameTeam.GameProcesses.Add(newGameProcess);
                }
                
                newGame.Teams.Add(gameTeam);
                newTeams.Add(gameTeam); // Dodaj do tymczasowej listy
            }

            // --- 4. Tworzenie wpisów w GameBoard ---
            foreach (var team in newTeams)
            {
                // a) Utwórz wpisy dla planszy głównej (jeden na każdy proces)
                foreach (var gameProcess in team.GameProcesses)
                {
                    _context.GameBoards.Add(new GameBoard
                    {
                        Game = newGame,
                        Team = team,
                        BoardId = newGame.TeamBoardId,
                        GameProcess = gameProcess, // Powiązanie z konkretnym procesem gry
                        PozX = 0,
                        PozY = 0
                    });
                }
        
                // b) Utwórz jeden wpis dla planszy konkurencji (bez powiązania z procesem)
                _context.GameBoards.Add(new GameBoard
                {
                    Game = newGame,
                    Team = team,
                    BoardId = newGame.RivalBoardId,
                    GameProcessId = null, // Jawne ustawienie braku procesu
                    PozX = 0,
                    PozY = 0
                });
            }

            // --- 5. Aktualizacja licencji i zapis do bazy ---
            user.LicensesOwned--;
            _context.Users.Update(user);

            // Zapisz wszystkie zmiany (Game, Teams, GameProcesses, GameBoards, User) w jednej operacji
            await _context.SaveChangesAsync();

            // Jeśli wszystko się udało, zatwierdź transakcję
            await transaction.CommitAsync();

            return Ok(new
            {
                gameId = newGame.GameId,
                message = "Gra utworzona pomyślnie."
            });
        }
        catch (Exception ex)
        {
            // W razie błędu wycofaj wszystkie zmiany
            await transaction.RollbackAsync();
            
            // Logowanie błędu jest bardzo ważne w środowisku produkcyjnym
            Console.WriteLine($"Błąd podczas tworzenia gry: {ex.InnerException?.Message ?? ex.Message}");
            
            return StatusCode(500, "Wystąpił błąd serwera podczas tworzenia gry. Zmiany zostały wycofane.");
        }
        }

        [HttpGet("{gameId}")]
        [Authorize]
        public async Task<IActionResult> GetGameById(int gameId)
        {
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [Authorize]
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<GameListItemDto>>> GetActiveGames()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int currentUserId))
            {
                return Unauthorized("Nie można zidentyfikować użytkownika lub nieprawidłowy format ID.");
            }

            var activeGames = await _context.Games
                .Where(g => g.UserId == currentUserId &&
                            (g.GameStatus == backend.Data.GameStatus.During ||
                             g.GameStatus == backend.Data.GameStatus.Paused))
                .Select(g => new GameListItemDto
                {
                    Id = g.GameId,
                    Name = g.GameDesc,
                    Status = g.GameStatus.ToString()
                })
                .ToListAsync();

            if (activeGames == null)
            {
                return Ok(new List<GameListItemDto>());
            }

            return Ok(activeGames);
        }

        [Authorize]
        [HttpPut("{gameId}/status")]
        public async Task<IActionResult> UpdateGameStatus(int gameId, [FromBody] string statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentUserId))
            {
                return Unauthorized("Nie można zidentyfikować użytkownika.");
            }

            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null)
            {
                return NotFound($"Gra o ID {gameId} nie została znaleziona.");
            }

            if (game.UserId != currentUserId)
            {
                return Forbid("Nie masz uprawnień do zmiany statusu tej gry.");
            }

            if (!Enum.TryParse<GameStatus>(statusDto, true, out GameStatus newGameStatus))
            {
                return BadRequest($"Nieprawidłowa wartość statusu: {statusDto}. Dozwolone: During, Paused, End.");
            }

            if (game.GameStatus == GameStatus.End && newGameStatus != GameStatus.End)
            {
                return BadRequest("Nie można zmienić statusu zakończonej gry.");
            }
            if (game.GameStatus == newGameStatus)
            {
                return Ok(new { message = $"Status gry to już '{newGameStatus}'. Bez zmian." });
            }


            game.GameStatus = newGameStatus;
            _context.Games.Update(game);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Status gry pomyślnie zmieniony na '{newGameStatus}'.", newStatus = newGameStatus.ToString() });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Wystąpił błąd serwera podczas aktualizacji statusu gry.");
            }
        }

        [HttpGet("get-token")]
        [Authorize]
        public async Task<IActionResult> GetTeamToken()
        {
            return Ok();
        }

        [HttpPost("stop-all")]
        [Authorize]
        public async Task<IActionResult> StopAllGames()
        {
            var adminUserName = User.Identity?.Name ?? "UnknownAdmin";

            try
            {
                var gamesToStop = await _context.Games
                    .Where(g => g.GameStatus == GameStatus.During)
                    .ToListAsync();

                if (!gamesToStop.Any())
                {
                    return Ok(new { message = "Nie znaleziono aktywnych gier do zatrzymania." });
                }

                foreach (var game in gamesToStop)
                {
                    game.GameStatus = GameStatus.Paused;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = $"Pomyślnie zatrzymano {gamesToStop.Count} gier (status zmieniony na 'Spauzowana')." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Wystąpił błąd serwera podczas próby zatrzymania gier.");
            }
        }
        [HttpPost("end-all")]
        [Authorize]
        public async Task<IActionResult> EndAllGames()
        {
            var adminUserName = User.Identity?.Name ?? "UnknownAdmin";
            try
            {
                var gamesToEnd = await _context.Games
                    .Where(g => g.GameStatus == GameStatus.During || g.GameStatus == GameStatus.Paused)
                    .ToListAsync();

                if (!gamesToEnd.Any())
                {
                    return Ok(new { message = "Nie znaleziono gier (w trakcie lub spauzowanych) do zakończenia." });
                }

                foreach (var game in gamesToEnd)
                {
                    game.GameStatus = GameStatus.End;

                }

                await _context.SaveChangesAsync();
                return Ok(new { message = $"Pomyślnie zakończono {gamesToEnd.Count} gier (status zmieniony na 'Zakończona')." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Wystąpił błąd serwera podczas próby zakończenia gier.");
            }
        }


        

        public static class TokenGenerator
        {
            private static readonly Random _random = new Random();
            private static readonly object _lock = new object();

            public static string GenerateRandomAlphanumericToken(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                char[] buffer = new char[length];
                lock (_lock)
                {
                    for (int i = 0; i < length; i++)
                    {
                        buffer[i] = chars[_random.Next(chars.Length)];
                    }
                }
                return new string(buffer);
            }
        }

    }
}