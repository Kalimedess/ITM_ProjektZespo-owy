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

public class CategorizedCardsDto
{
    public List<UnifiedCardDto> DecisionCards { get; set; } = new List<UnifiedCardDto>();
    public List<UnifiedCardDto> ItemCards { get; set; } = new List<UnifiedCardDto>();
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
        public async Task<ActionResult<CategorizedCardsDto>> GetUnifiedCardsForDeck(int deckId, [FromQuery] int gameId, [FromQuery] int teamId)
        {
            var deckExists = await _context.Decks.AnyAsync(d => d.DeckId == deckId);
            if (!deckExists)
            {
                return NotFound($"Talia o ID {deckId} nie została znaleziona.");
            }

            var playedCardIds = await _context.GameLogs
                .Where(gl => gl.GameId == gameId && gl.TeamId == teamId && gl.Status == true)
                .Select(gl => gl.CardId)
                .Distinct()
                .ToListAsync();

            var allRelevantEnablers = await _context.DecisionEnablers
                .Select(de => new { de.CardId, de.EnablerId })
                .Where(de => de.EnablerId.HasValue && !playedCardIds.Contains(de.EnablerId.Value))
                .ToListAsync();

            var enablersMap = allRelevantEnablers
                .GroupBy(de => de.CardId)
                .ToDictionary(g => g.Key, g => g.Select(de => de.EnablerId).ToList());

            // Pobieranie kart DECYZJI
            var decisionCardsInfo = await _context.Decisions
                .Where(d => d.DeckId == deckId && !playedCardIds.Contains(d.CardId))
                .Select(d => new { d.CardId, Title = d.DecisionShortDesc, Description = d.DecisionLongDesc, Cost = d.DecisionBaseCost })
                .OrderBy(c => c.CardId)
                .ToListAsync();
            
            // Pobieranie kart PRZEDMIOTÓW
            var itemCardsInfo = await _context.Items
                .Where(i => i.DeckId == deckId && !playedCardIds.Contains(i.CardId))
                .Select(i => new { i.CardId, Title = i.HardwareShortDesc, Description = i.HardwareLongDesc, Cost = i.ItemsBaseCost })
                .OrderBy(c => c.CardId)
                .ToListAsync();
            
            // Mapowanie DECYZJI na DTO
            var decisionCards = decisionCardsInfo.Select((c, index) => {
                enablersMap.TryGetValue(c.CardId, out var enablersForCard);
                return new UnifiedCardDto
                {
                    Id = c.CardId,
                    DeckId = deckId,
                    DisplayOrder = index + 1,
                    Title = c.Title,
                    Description = c.Description,
                    CardType = "Decision",
                    Cost = c.Cost,
                    Enablers = enablersForCard?.Where(id => id.HasValue).Select(id => id.Value).ToList() ?? new List<int>()
                };
            }).ToList();

            // Mapowanie PRZEDMIOTÓW na DTO
            var itemCards = itemCardsInfo.Select((c, index) => {
                enablersMap.TryGetValue(c.CardId, out var enablersForCard);
                return new UnifiedCardDto
                {
                    Id = c.CardId,
                    DeckId = deckId,
                    DisplayOrder = index + 1,
                    Title = c.Title,
                    Description = c.Description,
                    CardType = "Item",
                    Cost = c.Cost,
                    Enablers = enablersForCard?.Where(id => id.HasValue).Select(id => id.Value).ToList() ?? new List<int>()
                };
            }).ToList();
            
            // Złożenie ostateczn   ej odpowiedzi
            var result = new CategorizedCardsDto
            {
                DecisionCards = decisionCards,
                ItemCards = itemCards
            };

            return Ok(result);
        }

        [HttpGet("team/{teamToken}")]
        public async Task<IActionResult> GetPlayerSessionData(string teamToken)
        {
            if (string.IsNullOrWhiteSpace(teamToken) || teamToken.Length != 6)
            {
                return BadRequest(new { message = "Nieprawidłowy format tokena drużyny." });
            }

            var team = await _context.Teams
                .AsNoTracking()
                .Include(t => t.Game)
                    .ThenInclude(g => g.User)
                .Include(t => t.Game)
                    .ThenInclude(g => g.Deck)
                // ZMIANA 1: Poprawiono Include, aby pobierał dane planszy (`TeamBoard`).
                // Zakładam, że w modelu Game istnieje relacja/właściwość nawigacyjna o nazwie "TeamBoard".
                // Jeśli nazywa się inaczej (np. `Board`), zmień tę nazwę.
                .Include(t => t.Game)
                    .ThenInclude(g => g.TeamBoard)
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

            if (team.Game.GameStatus == GameStatus.End)
            {
                return BadRequest(new { message = "Ta gra została już zakończona." });
            }

            var gameBoardState = await _context.GameBoards
                .AsNoTracking()
                .FirstOrDefaultAsync(gb => gb.GameId == team.GameId && gb.TeamId == team.TeamId);

            if (team.Game.Deck == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Błąd konfiguracji gry: Brak danych talii." });
            }

            // ZMIANA: Zmieniłem warunek sprawdzający TeamBoard - teraz odwołuje się do bezpośredniej relacji.
            if (team.Game.TeamBoard == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Błąd konfiguracji gry: Brak danych planszy." });
            }

            var sessionData = new
            {
                gameId = team.Game.GameId,
                gameName = team.Game.GameDesc,
                gameStatus = team.Game.GameStatus?.ToString(),

                isOnline = team.Game.IsOnline, 

                teamId = team.TeamId,
                teamName = team.TeamName,
                teamColor = team.TeamColor,

                isIndependent = team.IsIndependent,

                // teamPositionX = gameBoardState?.PozX ?? 0,
                // teamPositionY = gameBoardState?.PozY ?? 0,

                deckId = team.Game.DeckId,
                deckName = team.Game.Deck.DeckName,

                // ZMIANA 2: Odkomentowano cały blok, aby dane planszy były wysyłane do frontendu.
                // To dostarczy `boardId`, którego brakowało.
                boardConfig = new
                {
                    boardId = team.Game.TeamBoard.BoardId,
                    Name = team.Game.TeamBoard.Name,
                    LabelsUp = team.Game.TeamBoard.LabelsUp?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                    LabelsRight = team.Game.TeamBoard.LabelsRight?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                    DescriptionDown = team.Game.TeamBoard.DescriptionDown,
                    DescriptionLeft = team.Game.TeamBoard.DescriptionLeft,
                    Rows = team.Game.TeamBoard.Rows,
                    Cols = team.Game.TeamBoard.Cols,
                    CellColor = team.Game.TeamBoard.CellColor,
                    BorderColor = team.Game.TeamBoard.BorderColor,
                    BorderColors = team.Game.TeamBoard.BorderColors?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>()
                },

                // DRUGI BOARD CONFIG DLA RYWALI BOARDA ZROBIC

                // ... inne potrzebne dane
            };

            return Ok(sessionData);
        }

        [HttpPost("getLogs")]
        public async Task<IActionResult> GetLogs([FromBody] LogData logData)
        {
            var gameLogs = await _context.GameLogs
                .Include(gl => gl.Feedback)
                .Where(gl => gl.GameId == logData.GameId && gl.TeamId == logData.TeamId)
                .OrderByDescending(gl => gl.Data)
                .Select(gl => new
                {
                    Data = gl.Data,
                    TeamName = gl.Team.TeamName,
                    GameId = gl.GameId,
                    CardId = gl.CardId,
                    FeedbackId = gl.FeedbackId,
                    FeedbackDescription = gl.Feedback.LongDescription,
                    Status = gl.Status,
                    Cost = gl.Cost,
                    MoveX = gl.MoveX,
                    MoveY = gl.MoveY
                })
                .ToListAsync();

            if (!gameLogs.Any())
            {
                return Ok(new List<object>()); // Zwracanie pustej listy jest lepsze niż 404 Not Found
            }

            return Ok(gameLogs);
        }

        [HttpGet("getTeamBit")]
        public async Task<IActionResult> GetTeamBit(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return NotFound($"Drużyna o ID {teamId} nie została znaleziona.");
            }
            return Ok(new { teamBud = team.TeamBud });
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
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == data.TeamId);
            if (team == null) return NotFound($"Drużyna o ID {data.TeamId} nie została znaleziona.");

            // ZMIANA: SPRAWDZAMY, CZY KARTA JEST PRZEDMIOTEM
            var isItem = await _context.Items.AnyAsync(i => i.CardId == selectedCard);
            
            bool finalStatus;
            double finalCost = data.Cost;

            if (isItem)
            {
                // JEŚLI TO PRZEDMIOT, ZAWSZE USTAWIAJ SUKCES I ZACHOWAJ KOSZT
                finalStatus = true;
            }
            else
            {
                // JEŚLI TO DECYZJA, UŻYJ WYNIKU Z FRONTENDU
                finalStatus = wasSuccess;
                if (!finalStatus) // Jeśli decyzja była nieudana, koszt jest zerowy
                {
                    finalCost = 0;
                }
            }

            // Szukamy feedbacku na podstawie OSTATECZNEGO statusu
            var feedback = await _context.Feedbacks
                 .FirstOrDefaultAsync(f => f.CardId == selectedCard && f.DeckId == data.DeckId && f.Status == finalStatus);

            var gameLogEntry = new GameLog
            {
                Data = DateTime.UtcNow,
                TeamId = data.TeamId,
                GameId = data.GameId,
                CardId = selectedCard,
                DeckId = data.DeckId,
                // Używamy BoardId z danych wejściowych, które jest już poprawne
                BoardId = data.BoardId, 
                GameProcessId = data?.GameProcessId,
                FeedbackId = feedback?.FeedbackId,
                // Używamy ostatecznego kosztu
                Cost = finalCost,
                // Używamy ostatecznego statusu
                Status = finalStatus, 
                MoveX = 0,
                MoveY = 0
            };
            _context.GameLogs.Add(gameLogEntry);

            team.TeamBud -= (int)finalCost;

            await _context.SaveChangesAsync();

            if (feedback != null)
            {
                return Ok(new
                {
                    message = $"Akcja karty przetworzona.",
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
                // Zwracamy ogólną wiadomość, która pasuje i do przedmiotów i do decyzji bez feedbacku
                return Ok(new { 
                    message = $"Akcja karty przetworzona.", 
                    newTeamBudget = team.TeamBud 
                });
            }
        }
    }
}