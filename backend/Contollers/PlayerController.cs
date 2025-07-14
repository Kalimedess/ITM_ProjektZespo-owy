using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Configuration;

public class UnifiedCardDto
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public int DisplayOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public double Cost { get; set; }
    public List<int> Enablers { get; set; } = new List<int>();
}

public class CardDataDTO
{
    public int GameId { get; set; }
    public int TeamId { get; set; }
    public int DeckId { get; set; }
    public int BoardId { get; set; }
    public int? GameProcessId { get; set; }
    public double Cost { get; set; }
}

public class LogData
{
    public int GameId { get; set; }
    public int TeamId { get; set; }
}

namespace backend.Controllers
{
    [Route("api/player/")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("deck/{deckId}/unified-cards")]
        public async Task<ActionResult<IEnumerable<UnifiedCardDto>>> GetUnifiedCardsForDeck(int deckId, [FromQuery] int gameId, [FromQuery] int teamId)
        {
            var deckExists = await _context.Decks.AnyAsync(d => d.DeckId == deckId);
            if (!deckExists)
            {
                return NotFound($"Talia o ID {deckId} nie została znaleziona.");
            }

            var playedCardIds = await _context.GameLogs
                .Where(gl => gl.GameId == gameId && gl.TeamId == teamId && gl.Status == true)
                .Select(gl => gl.CardId) // Pobieramy tylko ID zagranych kart
                .Distinct() // Unikalne ID kart
                .ToListAsync();

            var allRelevantEnablers = await _context.DecisionEnablers
                .Select(de => new { de.CardId, de.EnablerId })
                .Where(de => de.EnablerId.HasValue && !playedCardIds.Contains(de.EnablerId.Value))
                .ToListAsync();

            var enablersMap = allRelevantEnablers
                .GroupBy(de => de.CardId)
                .ToDictionary(g => g.Key, g => g.Select(de => de.EnablerId).ToList());

            var decisionCardsInfo = await _context.Decisions
                .Where(d => d.DeckId == deckId && !playedCardIds.Contains(d.CardId))
                .Select(d => new
                {
                    d.CardId,
                    DeckId = deckId,
                    Title = d.DecisionShortDesc,
                    Description = d.DecisionLongDesc,
                    Type = "Decision",
                    Cost = d.DecisionBaseCost,
                })
                .ToListAsync();

            var itemCardsInfo = await _context.Items
                .Where(i => i.DeckId == deckId && !playedCardIds.Contains(i.CardId))
                .Select(i => new
                {
                    i.CardId,
                    DeckId = deckId,
                    Title = i.HardwareShortDesc,
                    Description = i.HardwareLongDesc,
                    Type = "Item",
                    Cost = i.ItemsBaseCost,
                })
                .ToListAsync();


            var allCardsTemp = decisionCardsInfo
                .Concat(itemCardsInfo)
                .OrderBy(c => c.CardId)
                .ToList();

            var unifiedCards = allCardsTemp
                .Select((c, index) => {
                    List<int> enablersForCard = new List<int>();
                    if (enablersMap.TryGetValue(c.CardId, out var enablerList))
                    {
                        enablersForCard = enablerList.Where(id => id.HasValue).Select(id => id.Value).ToList();
                    }

                    return new UnifiedCardDto
                    {
                        Id = c.CardId,
                        DeckId = deckId,
                        DisplayOrder = index + 1,
                        Title = c.Title,
                        Description = c.Description,
                        CardType = c.Type,
                        Cost = c.Cost,
                        Enablers = enablersForCard
                    };
                })
                .ToList();

            return Ok(unifiedCards);
        }

        [HttpGet("team/{teamToken}")]
        public async Task<IActionResult> GetPlayerSessionData(string teamToken)
        {
            if (string.IsNullOrWhiteSpace(teamToken) || teamToken.Length != 6) // TeamToken może mieć mniej niż 6, jeśli tak generujesz
            {
                return BadRequest(new { message = "Nieprawidłowy format tokena drużyny."});
            }

            // Znajdź drużynę na podstawie tokena
            // Dołącz powiązane dane gry, talii, planszy oraz stan GameBoard dla tej drużyny
            var team = await _context.Teams
                .AsNoTracking() // Zazwyczaj sesja gracza to tylko odczyt
                .Include(t => t.Game)
                    .ThenInclude(g => g.User) // Admin/twórca gry
                .Include(t => t.Game)
                    .ThenInclude(g => g.Deck) // Talia używana w grze
                .Include(t => t.Game)
                    .ThenInclude(g => g.GameBoards) // Plansza używana w grze
                .FirstOrDefaultAsync(t => t.TeamToken == teamToken);

            Console.Write("Znaleziono drużynę");

            if (team == null)
            {
                return NotFound(new { message = "Nie znaleziono drużyny dla podanego tokena." });
            }

            if (team.Game == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Błąd danych: Drużyna nie jest przypisana do gry." });
            }

            // Sprawdzenie statusu gry - np. czy nie jest zakończona
            if (team.Game.GameStatus == GameStatus.End)
            {
                // Możesz zwrócić specyficzny obiekt dla zakończonej gry lub błąd
                return BadRequest(new { message = "Ta gra została już zakończona." });
            }
            // Możesz dodać podobne sprawdzenie dla statusu Paused, jeśli to ma blokować dostęp

            // Pobierz dane GameBoard dla tej drużyny i gry, jeśli istnieją
            // Zakładam, że GameBoard ma klucz złożony (GameId, TeamId) lub TeamId jest unikalne w ramach gry dla GameBoard.
            // Jeśli GameBoard jest 1-do-1 z Team w ramach gry, to można by dać relację bezpośrednio z Team do GameBoard.
            // Tutaj założę, że musimy go wyszukać.
            var gameBoardState = await _context.GameBoards
                .AsNoTracking()
                .FirstOrDefaultAsync(gb => gb.GameId == team.GameId && gb.TeamId == team.TeamId);

            // Zabezpieczenie, jeśli gra nie ma np. talii lub planszy (choć powinna być walidacja przy tworzeniu gry)
            if (team.Game.Deck == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Błąd konfiguracji gry: Brak danych talii." });
            }
            if (team.Game.GameBoards == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Błąd konfiguracji gry: Brak danych planszy." });
            }


            // Przygotuj obiekt odpowiedzi dla frontendu
            var sessionData = new
            {
                gameId = team.Game.GameId,
                gameName = team.Game.GameDesc, // Używam GameDesc jako nazwy gry
                gameStatus = team.Game.GameStatus?.ToString(), // Bezpieczne odwołanie i konwersja enum

                teamId = team.TeamId,
                teamName = team.TeamName,
                teamColor = team.TeamColor,

                // Pozycja pionka z GameBoard, jeśli istnieje, inaczej domyślne
                // teamPositionX = gameBoardState?.PozX ?? 0,
                // teamPositionY = gameBoardState?.PozY ?? 0,

                deckId = team.Game.DeckId,
                deckName = team.Game.Deck.DeckName,

                

                // boardConfig = new
                // {
                //     boardId = gameBoardState.BoardId, 
                //     Name = team.Game.TeamBoard.Name,
                //     LabelsUp = team.Game.TeamBoard.LabelsUp?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                //     LabelsRight = team.Game.TeamBoard.LabelsRight?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                //     DescriptionDown = team.Game.TeamBoard.DescriptionDown,
                //     DescriptionLeft = team.Game.TeamBoard.DescriptionLeft,
                //     Rows = team.Game.TeamBoard.Rows,
                //     Cols = team.Game.TeamBoard.Cols,
                //     CellColor = team.Game.TeamBoard.CellColor,
                //     BorderColor = team.Game.TeamBoard.BorderColor,
                //     BorderColors = team.Game.TeamBoard.BorderColors?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>()
                // },

                // currentTurn = team.Game.CurrentTurn, // Jeśli Game ma takie pole
                // currentQuestion = gameBoardState?.CurrentQuestionText, // Jeśli GameBoard lub GameProcess ma pytanie
                // ... inne potrzebne dane dla gracza z Game, Team, GameBoard, GameProcess itp.
            };

            return Ok(sessionData);
        }

        [HttpPost("getLogs")]
        public async Task<IActionResult> GetLogs([FromBody] LogData logData)
        {
            var gameLogs = await _context.GameLogs
                .Include(gl => gl.Feedback)
                .Where(gl => gl.GameId == logData.GameId && gl.TeamId == logData.TeamId)
                .OrderByDescending(gl => gl.Data) // Najnowsze logi na początku (lub OrderBy(gl => gl.Data) dla najstarszych)
                .Select(gl => new
                {
                    Data = gl.Data,
                    TeamName = gl.Team.TeamName,
                    GameId = gl.GameId,
                    CardId = gl.CardId,
                    FeedbackId = gl.FeedbackId,
                    FeedbackDescription = gl.Feedback.LongDescription, // Wymaga .Include(gl => gl.Feedback)
                    Status = gl.Status,
                    Cost = gl.Cost,
                    MoveX = gl.MoveX,
                    MoveY = gl.MoveY

                })
                .ToListAsync();

            if (!gameLogs.Any())
            {
                return Ok(new { message = "No game logs found for GameId: {GameId}, TeamId: {TeamId}", logData.GameId, logData.TeamId });
                // Zwróć pustą listę zamiast 404, jeśli brak logów jest normalnym stanem
                // return NotFound(new { message = "Nie znaleziono logów dla podanej gry i drużyny." });
            }

            return Ok(gameLogs);
        }

        [HttpGet("getCurrency")]
        public async Task<IActionResult> GetTeamBit(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);

            return Ok(new {teamBud = team.TeamBud});
        }


        [HttpPost("success/{selectedCard}")]
        public async Task<IActionResult> SendSuccess(int selectedCard, [FromBody] CardDataDTO cardData)
        {
            if (cardData == null) return BadRequest("Brak danych w żądaniu.");

            return await ProcessCardPlay(selectedCard, true, cardData);
        }

        [HttpPost("failure/{selectedCard}")]
        public async Task<IActionResult> SendFailure(int selectedCard, [FromBody] CardDataDTO cardData)
        {
            if (cardData == null) return BadRequest("Brak danych w żądaniu.");

            return await ProcessCardPlay(selectedCard, false, cardData);
        }

        private async Task<IActionResult> ProcessCardPlay(int selectedCard, bool wasSuccess, CardDataDTO? data = null)
        {
            var feedback = await _context.Feedbacks
                 .FirstOrDefaultAsync(f => f.CardId == selectedCard && f.DeckId == data.DeckId && f.Status == wasSuccess);

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == data.TeamId);

            if (wasSuccess != true) data.Cost = 0;

            var gameLogEntry = new GameLog
            {
                Data = DateTime.UtcNow,
                TeamId = data.TeamId,
                GameId = data.GameId,
                CardId = selectedCard,
                DeckId = data.DeckId,
                BoardId = data.BoardId,
                GameProcessId = data?.GameProcessId, // Może być null
                FeedbackId = feedback?.FeedbackId,
                Cost = data.Cost,
                Status = wasSuccess,
                MoveX = 0,
                MoveY = 0
            };
            _context.GameLogs.Add(gameLogEntry);

            team.TeamBud -= (int)data.Cost;

            await _context.SaveChangesAsync();

            if (feedback != null)
            {
                return Ok(new
                {
                    message = $"Akcja karty przetworzona ({(wasSuccess ? "Sukces" : "Porażka")}).",
                    feedback = new
                    {
                        feedback.FeedbackId,
                        feedback.LongDescription,
                        feedback.Status
                    },
                    newTeamBudget = team.TeamBud
                });
            }
            else
            {
                return Ok(new { message = $"Akcja karty przetworzona ({(wasSuccess ? "Sukces" : "Porażka")}), ale nie znaleziono specyficznego feedbacku.", newTeamBudget = team.TeamBud });
            }
        }

    }
}