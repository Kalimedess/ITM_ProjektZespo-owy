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
    public List<ProcessDto> Process { get; set; } = new List<ProcessDto>();
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

            var newGame = new Game
            {
                GameDesc = gameDto.GameName,
                TeamBoardId = gameDto.BoardId,
                RivalBoardId = gameDto.RivalBoardId,
                DeckId = gameDto.DeckId,
                GameStatus = GameStatus.During,
                UserId = userId,
                IsOnline = gameDto.GameMode,
                Teams = new List<Team>()
            };


            foreach (var teamDto in gameDto.Teams)
            {
                var gameTeam = new Team
                {
                    TeamName = teamDto.Name,
                    TeamColor = teamDto.Colour,
                    TeamBud = gameDto.StartBits,
                    TeamToken = TokenGenerator.GenerateRandomAlphanumericToken(6),
                    IsIndependent= teamDto.IsAbleToMakeDecisions,
                    GameProcesses = new List<GameProcess>()
                };

                foreach (var processDto in gameDto.Process)
                {
                    var newProcess = new GameProcess
                    {
                        ProcessDesc = processDto.ShortName,
                        ProcessLongDesc = processDto.Name,
                        Team = gameTeam,
                        Game = newGame,
                        
                        
                    };
                    gameTeam.GameProcesses.Add(newProcess);
                }
                newGame.Teams.Add(gameTeam);
            }

            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();

            var gameBoardsToCreate = new List<GameBoard>();
             foreach (var team in newGame.Teams)
            {
                // a) Stwórz 5 wpisów dla planszy głównej, po jednym dla każdego z unikalnych procesów tej drużyny
                foreach (var process in team.GameProcesses)
                {
                    gameBoardsToCreate.Add(new GameBoard
                    {
                        GameId = newGame.GameId,
                        TeamId = team.TeamId,
                        BoardId = newGame.TeamBoardId,
                        GameProcessId = process.GameProcessId,
                        PozX = 0,
                        PozY = 0
                    });
                }
        
                // b) Stwórz 1 wpis dla planszy konkurencji (bez procesu)
                gameBoardsToCreate.Add(new GameBoard
                {
                    GameId = newGame.GameId,
                    TeamId = team.TeamId,
                    BoardId = newGame.RivalBoardId,
                    GameProcessId = null,
                    PozX = 0,
                    PozY = 0
                });
            }
            await _context.GameBoards.AddRangeAsync(gameBoardsToCreate);

            user.LicensesOwned--;
            _context.Users.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Błąd zapisu do DB: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, "Wystąpił błąd serwera podczas tworzenia gry.");
            }

            return Ok(new
            {
                gameId = newGame.GameId,
                message = "Gra utworzona pomyślnie."
            });
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