using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Configuration;
using Microsoft.AspNetCore.SignalR;
using DocumentFormat.OpenXml.Office2010.Excel;
using backend.Services;

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

    // Ominięcie sugerowania u admina
    public bool ForceExecution { get; set; } = false;
}

public class LogData
{
    public int GameId { get; set; }
    public int TeamId { get; set; }
}

// DTO do przesyłania danych o drużynie dla panelu zarządzania
public class TeamManagementDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int TeamBud { get; set; }
    public int BoardId { get; set; } 
    public int? DeckId { get; set; }
    public string TeamToken { get; set; }
}

// DTO do odbierania nowej wartości budżetu
public class UpdateBudgetDto
{
    public int NewBudget { get; set; }
}

public class CardInfoDto
{
    public int CardId { get; set; }
    public string CardName { get; set; } = string.Empty;
}

public class UnlockCardDto
{
    public int CardId { get; set; }
    public int TeamId { get; set; }
}

public class CategorizedCardsDto
{
    public List<UnifiedCardDto> DecisionCards { get; set; } = new List<UnifiedCardDto>();
    public List<UnifiedCardDto> ItemCards { get; set; } = new List<UnifiedCardDto>();
}

public class GameEventDto
{
    public int EventId { get; set; }
    public string ShortDesc { get; set; } = string.Empty;
    public string LongDesc { get; set; } = string.Empty;
}

public class ApplyEventDto
{
    public int EventId { get; set; }
}

namespace backend.Controllers
{
    [Route("api/player/")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IPlayerService _playerService;

        public PlayerController(AppDbContext context, IPlayerService playerService, IHubContext<GameHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _playerService = playerService;
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
            var decisionCards = decisionCardsInfo.Select((c, index) =>
            {
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
            var itemCards = itemCardsInfo.Select((c, index) =>
            {
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
                return BadRequest(new { message = "Nieprawidłowy format tokena drużyny." });
            }

            var team = await _context.Teams
                .AsNoTracking()
                .Include(t => t.Game)
                    .ThenInclude(g => g.User)
                .Include(t => t.Game)
                    .ThenInclude(g => g.Deck)
                .Include(t => t.Game)
                    .ThenInclude(g => g.TeamBoard)
                .Include(t => t.Game)
                    .ThenInclude(g => g.RivalBoard)
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

                rivalBoardConfig = team.Game.RivalBoard != null ? new
                {
                    boardId = team.Game.RivalBoard.BoardId,
                    Name = team.Game.RivalBoard.Name,
                    LabelsUp = team.Game.RivalBoard.LabelsUp?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                    LabelsRight = team.Game.RivalBoard.LabelsRight?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>(),
                    DescriptionDown = team.Game.RivalBoard.DescriptionDown,
                    DescriptionLeft = team.Game.RivalBoard.DescriptionLeft,
                    Rows = team.Game.RivalBoard.Rows,
                    Cols = team.Game.RivalBoard.Cols,
                    CellColor = team.Game.RivalBoard.CellColor,
                    BorderColor = team.Game.RivalBoard.BorderColor,
                    BorderColors = team.Game.RivalBoard.BorderColors?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray() ?? Array.Empty<string>()
                } : null

                // ... inne potrzebne dane
            };

            return Ok(sessionData);
        }


        [HttpGet("team-board")]
        public async Task<IActionResult> GetTeamBoardData([FromQuery] int gameId, [FromQuery] int teamId, [FromQuery] int boardId)
        {
            // Pobieramy pionki reprezentujące PROCESY dla konkretnej drużyny na jej planszy
            var processPawns = await _context.GameBoards
                .Where(gb =>
                    gb.GameId == gameId &&
                    gb.TeamId == teamId &&
                    gb.BoardId == boardId &&
                    gb.GameProcessId != null) // Warunek: POBIERZ PIONKI Z PROCESEM
                .Select(gb => new
                {
                    // Spójne nazewnictwo (PascalCase)
                    GPId = gb.GameProcessId,
                    PosX = gb.PozX,
                    PosY = gb.PozY,
                    // Pionki procesów mogą mieć kolor zdefiniowany w procesie, nie w drużynie
                    Color = gb.GameProcess.Process.ProcessColor,
                    Name = gb.GameProcess.Process.ProcessDesc
                })
                .ToListAsync();

            return Ok(processPawns);
        }

        [HttpGet("rival-board")]
        public async Task<IActionResult> GetRivalBoardData([FromQuery] int gameId, [FromQuery] int boardId)
        {
            var RivalPawns = await _context.GameBoards
                .Where(gb =>
                    gb.GameId == gameId &&
                    gb.BoardId == boardId &&
                    gb.GameProcessId == null)
                .Include(gb => gb.Team)
                .Select(gb => new
                {
                    PosX = gb.PozX,
                    PosY = gb.PozY,
                    TeamColor = gb.Team.TeamColor,
                    TeamId = gb.Team.TeamId,
                    TeamName = gb.Team.TeamName
                })
                .ToListAsync();

            return Ok(RivalPawns);
        }


        [HttpPost("getLogs")]
        public async Task<IActionResult> GetLogs([FromBody] LogData logData, [FromQuery] bool onlyPending = false)
        {
            var query = _context.GameLogs
                .Include(gl => gl.Team)
                .Include(gl => gl.Feedback)
                .Where(gl => gl.GameId == logData.GameId);

            if (logData.TeamId > 0)
            {
                query = query.Where(gl => gl.TeamId == logData.TeamId);
            }

            if (onlyPending)
            {
                query = query.Where(gl => gl.IsApproved == false);
            }
            else
            {
                query = query.Where(gl => gl.IsApproved != false);
            }

            var gameLogs = await query.OrderByDescending(gl => gl.Data).ToListAsync();

            if (!gameLogs.Any()) return Ok(new List<object>());

            // Zbierz tylko te ID, które nie są null
            var cardIds = gameLogs.Where(gl => gl.CardId.HasValue).Select(gl => gl.CardId.Value).Distinct().ToList();

            var decisions = await _context.Decisions.Where(d => cardIds.Contains(d.CardId)).ToListAsync();
            var decisionTitles = decisions.GroupBy(d => d.CardId).ToDictionary(g => g.Key, g => g.First().DecisionShortDesc);

            var items = await _context.Items.Where(i => cardIds.Contains(i.CardId)).ToListAsync();
            var itemTitles = items.GroupBy(i => i.CardId).ToDictionary(g => g.Key, g => g.First().HardwareShortDesc);

            var result = gameLogs.Select(gl =>
            {
                string? cardTitle = null;
                // --- POPRAWKA TUTAJ ---
                // Sprawdzamy, czy CardId ma wartość, zanim użyjemy go do przeszukania słownika
                if (gl.CardId.HasValue)
                {
                    decisionTitles.TryGetValue(gl.CardId.Value, out var decisionTitle);
                    itemTitles.TryGetValue(gl.CardId.Value, out var itemTitle);
                    cardTitle = decisionTitle ?? itemTitle;
                }

                return new
                {
                    LogId = gl.GameLogId, // Zakładam, że model GameLog ma pole Id
                    GameEventId = gl.GameEventId,
                    Timestamp = gl.Data,
                    TeamId = gl.TeamId,
                    TeamName = gl.Team?.TeamName,
                    CardId = gl.CardId,
                    CardTitle = cardTitle ?? "Zdarzenie systemowe", // Jeśli nazwa nie została znaleziona
                    FeedbackDescription = gl.Feedback?.LongDescription,
                    Status = gl.Status,
                    Cost = gl.Cost,
                };
            });

            return Ok(result);
        }

        [HttpGet("getCurrency")]
        public async Task<IActionResult> GetCurrency(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);

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
            var team = await _context.Teams.FindAsync(data.TeamId);
            if (team == null) return NotFound($"Drużyna o ID {data.TeamId} nie została znaleziona.");

            var isItem = await _context.Items.AnyAsync(i => i.CardId == selectedCard);
            double finalCost = data.Cost;
            int? eventIdForLog = null;

            // --- NOWA LOGIKA OBSŁUGI ZDARZEŃ ---
            if (team.GameEventId.HasValue && team.TurnsLeft > 0)
            {
                var activeEvent = await _context.GameEvents.FindAsync(team.GameEventId.Value);
                if (activeEvent != null)
                {
                    bool eventApplies = false;
                    // Sprawdzamy, czy event dotyczy tego typu karty
                    if (isItem && activeEvent.ItemsCostWeight.HasValue)
                    {
                        // To jest przedmiot, a event ma modyfikator kosztu przedmiotów
                        finalCost = data.Cost * (1 + activeEvent.ItemsCostWeight.Value);
                        eventApplies = true;
                    }
                    else if (!isItem && activeEvent.DecisionCostWeight.HasValue)
                    {
                        // To jest decyzja, a event ma modyfikator kosztu decyzji
                        finalCost = data.Cost * (1 + activeEvent.DecisionCostWeight.Value);
                        eventApplies = true;
                    }

                    // Sprawdzamy, czy event dotyczy boosterów (działa na każdą kartę)
                    if (activeEvent.BoosterX.HasValue || activeEvent.BoosterY.HasValue)
                    {
                        eventApplies = true;
                        // Logika boosterów zostanie dodana w przyszłości, ale event się liczy
                    }

                    if (eventApplies)
                    {
                        // Jeśli event został zastosowany, zmniejszamy licznik i oznaczamy ID do zapisu w logu
                        team.TurnsLeft--;
                        eventIdForLog = team.GameEventId.Value;

                        // Jeśli to było ostatnie użycie, dezaktywuj event
                        if (team.TurnsLeft <= 0)
                        {
                            team.GameEventId = null;
                            team.TurnsLeft = 0;
                        }
                    }
                }
            }
            // --- KONIEC LOGIKI ZDARZEŃ ---

            bool finalStatus;

            var specialEnablerUsed = await _context.DecisionEnablers.AnyAsync(de =>
                de.CardId == selectedCard &&
                de.GameId == data.GameId &&
                de.TeamId == data.TeamId &&
                de.EnablerId == null);

            if (specialEnablerUsed)
            {
                finalStatus = true;
            }
            else if (isItem)
            {
                finalStatus = true;
            }
            else
            {
                finalStatus = wasSuccess;
            }

            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.CardId == selectedCard && f.DeckId == data.DeckId && f.Status == finalStatus);

            var gameLogEntry = new GameLog
            {
                Data = DateTime.UtcNow,
                TeamId = data.TeamId,
                GameId = data.GameId,
                CardId = selectedCard,
                DeckId = data.DeckId,
                BoardId = data.BoardId,
                FeedbackId = feedback?.FeedbackId,
                Cost = finalCost, // Używamy finalnego, zmodyfikowanego kosztu
                GameEventId = eventIdForLog, // Zapisujemy ID eventu, jeśli był użyty
                Status = finalStatus,
                IsApproved = data.ForceExecution ? (bool?)null : (team.IsIndependent ? (bool?)null : false)
            };
            _context.GameLogs.Add(gameLogEntry);

            await _context.SaveChangesAsync();

            // Przekazujemy zmodyfikowany koszt do metody wykonującej efekty
            var originalCostForEffects = data.Cost; // Zachowujemy oryginalny, na wszelki wypadek
            data.Cost = finalCost; // Nadpisujemy koszt w DTO

            if (team.IsIndependent || data.ForceExecution)
            {
                await ExecuteCardEffects(gameLogEntry);
            }

            await _context.SaveChangesAsync();

            if (team.IsIndependent || data.ForceExecution)
            {
                // Jeśli akcja została wykonana, odśwież historię
                await _hubContext.Clients.Group($"game-{gameLogEntry.GameId}").SendAsync("HistoryUpdated");
            }
            else
            {
                // Jeśli to była tylko sugestia, odśwież listę oczekujących
                await _hubContext.Clients.Group($"game-{gameLogEntry.GameId}").SendAsync("PendingUpdated");
            }

            return Ok(new
            {
                message = (team.IsIndependent || data.ForceExecution) ? "Akcja karty została wykonana." : "Sugestia zagrania karty została wysłana do zatwierdzenia.",
                newTeamBudget = team.TeamBud
            });
        }

        private async Task ExecuteCardEffects(GameLog log)
        {
            // --- Krok 1: Zastosuj efekty związane z drużyną (np. budżet) ---
            var trackedTeam = await _context.Teams.FindAsync(log.TeamId);
            if (trackedTeam == null)
            {
                // To nie powinno się zdarzyć, jeśli dane są spójne. Rzuć wyjątek lub zaloguj krytyczny błąd.
                throw new InvalidOperationException($"Drużyna o ID {log.TeamId} nie istnieje.");
            }

            // Odejmij koszt karty od budżetu drużyny.
            trackedTeam.TeamBud -= (int)(log.Cost ?? 0); // Używamy ?? 0 dla bezpieczeństwa, gdyby koszt był null

            // --- Krok 2: Zaktualizuj status logu, jeśli była to sugestia ---
            // Logika jest taka sama, ale komentarz wyjaśnia intencję.
            // Jeśli log był sugestią (IsApproved == false), zatwierdź go.
            if (log.IsApproved == false)
            {
                log.IsApproved = true;
            }

            // --- Krok 3: Usuń specjalny enabler, jeśli został użyty do zagrania tej karty ---
            // Zapytanie jest takie samo jak wcześniej.
            var specialEnabler = await _context.DecisionEnablers.FirstOrDefaultAsync(de =>
                de.CardId == log.CardId &&
                de.GameId == log.GameId &&
                de.TeamId == log.TeamId &&
                de.EnablerId == null);

            if (specialEnabler != null)
            {
                _context.DecisionEnablers.Remove(specialEnabler);
            }

            // --- Krok 4: Zastosuj ruchy na planszy, uwzględniając eventy ---
            // Jeśli zagranie karty zakończyło się sukcesem, dodaj powiązane ruchy.
            if (log.Status == true)
            {
                // Pobierz aktywny event, jeśli log był z nim powiązany
                GameEvent? activeEvent = null;
                if (log.GameEventId.HasValue)
                {
                    activeEvent = await _context.GameEvents.FindAsync(log.GameEventId.Value);
                }

                // Wywołaj metodę odpowiedzialną TYLKO za dodanie specyfikacji i ruchów
                await AddGameLogSpecsForCard(log, activeEvent);
            }
        }

        [HttpGet("game/{gameId}/teams-management")]
        public async Task<ActionResult<IEnumerable<TeamManagementDto>>> GetTeamsForManagement(int gameId)
        {
            var game = await _context.Games.Include(g => g.TeamBoard).FirstOrDefaultAsync(g => g.GameId == gameId);
            if (game == null || game.TeamBoard == null)
            {
                return NotFound("Nie znaleziono gry lub gra nie ma przypisanej planszy.");
            }
            var boardId = game.TeamBoard.BoardId;
            var deckId = game.DeckId;

            var teams = await _context.Teams
                .Where(t => t.GameId == gameId)
                .Select(t => new TeamManagementDto
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    TeamBud = t.TeamBud,
                    BoardId = boardId,
                    DeckId = deckId,
                    TeamToken = t.TeamToken
                })
                .OrderBy(t => t.TeamName)
                .ToListAsync();

            return Ok(teams);
        }

        [HttpPut("team/{teamId}/budget")]
        public async Task<IActionResult> UpdateTeamBudget(int teamId, [FromBody] UpdateBudgetDto budgetDto)
        {
            if (budgetDto == null)
            {
                return BadRequest("Nie podano danych do aktualizacji.");
            }

            var team = await _context.Teams.FindAsync(teamId);

            if (team == null)
            {
                return NotFound($"Drużyna o ID {teamId} nie została znaleziona.");
            }

            team.TeamBud = budgetDto.NewBudget;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Budżet drużyny '{team.TeamName}' został zaktualizowany.",
                newBudget = team.TeamBud
            });
        }

        [HttpGet("game/{gameId}/decision-cards")]
        public async Task<ActionResult<IEnumerable<CardInfoDto>>> GetDecisionCardsForGame(int gameId)
        {
            // Znajdź grę, aby uzyskać DeckId
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return NotFound($"Gra o ID {gameId} nie została znaleziona.");
            }

            if (game.DeckId == null)
            {
                return BadRequest($"Gra o ID {gameId} nie ma przypisanej talii.");
            }

            var deckId = game.DeckId;

            // Pobierz tylko karty decyzji dla tej talii
            var decisionCards = await _context.Decisions
                .Where(d => d.DeckId == deckId)
                .OrderBy(d => d.CardId)
                .Select(d => new CardInfoDto
                {
                    CardId = d.CardId,
                    CardName = d.DecisionShortDesc // Używamy krótkiego opisu jako nazwy
                })
                .OrderBy(c => c.CardName)
                .ToListAsync();

            return Ok(decisionCards);
        }

        [HttpGet("game/{gameId}/item-cards")]
        public async Task<ActionResult<IEnumerable<CardInfoDto>>> GetItemCardsForGame(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return NotFound($"Gra o ID {gameId} nie została znaleziona.");
            }

            if (game.DeckId == null)
            {
                return BadRequest($"Gra o ID {gameId} nie ma przypisanej talii.");
            }

            var deckId = game.DeckId;

            // Pobierz tylko karty przedmiotów dla tej talii
            var itemCards = await _context.Items
                .Where(i => i.DeckId == deckId)
                .Select(i => new CardInfoDto
                {
                    CardId = i.CardId,
                    CardName = i.HardwareShortDesc // Używamy krótkiego opisu jako nazwy
                })
                .OrderBy(c => c.CardName)
                .ToListAsync();

            return Ok(itemCards);
        }

        [HttpPost("game/{gameId}/unlock-card")]
        public async Task<IActionResult> UnlockCardForTeam(int gameId, [FromBody] UnlockCardDto unlockData)
        {
            if (unlockData == null)
            {
                return BadRequest("Brak danych do odblokowania karty.");
            }

            // Sprawdźmy, czy taki wpis już nie istnieje, aby uniknąć duplikatów
            // Porównujemy też EnablerId, które musi być null
            var alreadyExists = await _context.DecisionEnablers.AnyAsync(de =>
                de.CardId == unlockData.CardId &&
                de.TeamId == unlockData.TeamId &&
                de.GameId == gameId &&
                de.EnablerId == null);

            if (alreadyExists)
            {
                // Zamiast błędu, możemy po prostu zwrócić sukces, bo cel został osiągnięty
                return Ok(new { message = "Ta karta jest już odblokowana dla tej drużyny." });
            }

            // Tworzymy nowy wpis "odblokowujący"
            var newEnabler = new DecisionEnabler
            {
                CardId = unlockData.CardId,
                TeamId = unlockData.TeamId,
                GameId = gameId,
                EnablerId = null // Kluczowy element - EnablerId jest pusty (NULL)
            };

            _context.DecisionEnablers.Add(newEnabler);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Karta (ID: {unlockData.CardId}) została pomyślnie odblokowana dla drużyny (ID: {unlockData.TeamId})." });
        }

        [HttpGet("game/{gameId}/pending-logs")]
        public async Task<IActionResult> GetPendingLogs(int gameId)
        {
            // Użyjemy tej samej logiki co w GetLogs, aby zwrócić spójne dane
            var logData = new LogData { GameId = gameId, TeamId = 0 };
            return await GetLogs(logData, onlyPending: true);
        }

        [HttpPost("approve-log/{logId}")]
        public async Task<IActionResult> ApproveLog(int logId)
        {
            var logToApprove = await _context.GameLogs.FindAsync(logId);
            if (logToApprove == null)
            {
                return NotFound("Nie znaleziono logu do zatwierdzenia.");
            }
            if (logToApprove.IsApproved != false)
            {
                return BadRequest("Ten log nie oczekuje na zatwierdzenie.");
            }

            await ExecuteCardEffects(logToApprove);
            await _context.SaveChangesAsync();

            var gameId = logToApprove.GameId.ToString();

            // Odśwież listę oczekujących (bo jedna sugestia zniknęła)
            await _hubContext.Clients.Group($"game-{logToApprove.GameId}").SendAsync("PendingUpdated");
            // Odśwież historię (bo pojawił się w niej nowy wpis)
            await _hubContext.Clients.Group($"game-{logToApprove.GameId}").SendAsync("HistoryUpdated");
            // Odśwież planszę
            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("BoardUpdated");

            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("BoardUpdated", new { teamId = logToApprove.TeamId });

            return Ok(new { message = "Sugestia została zatwierdzona." });
        }

        [HttpDelete("reject-log/{logId}")]
        public async Task<IActionResult> RejectLog(int logId)
        {
            var logToReject = await _context.GameLogs.FindAsync(logId);
            if (logToReject == null)
            {
                return NotFound("Nie znaleziono logu do odrzucenia.");
            }
            if (logToReject.IsApproved != false)
            {
                return BadRequest("Ten log nie oczekuje na zatwierdzenie.");
            }

            _context.GameLogs.Remove(logToReject);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group($"game-{logToReject.GameId}").SendAsync("PendingUpdated");

            return Ok(new { message = "Sugestia została odrzucona." });
        }

        [HttpGet("game-events")]
        public async Task<ActionResult<IEnumerable<GameEventDto>>> GetGameEventsForCurrentUser()
        {
            // Pobieramy ID zalogowanego użytkownika z jego "claimów" (oświadczeń) w tokenie.
            // Zakładam, że claim z ID użytkownika nazywa się "nameid" lub "sub". Może to być też "Id".
            // To standard dla JWT i ASP.NET Core Identity.
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Jeśli nie uda się znaleźć ID użytkownika, oznacza to problem z autoryzacją.
                return Unauthorized("Nie udało się zidentyfikować użytkownika.");
            }

            var gameEvents = await _context.GameEvents
                .Where(ge => ge.UserId == userId)
                .Select(ge => new GameEventDto
                {
                    EventId = ge.GameEventId,
                    ShortDesc = ge.EventShortDesc,
                    LongDesc = ge.EventLongDesc
                })
                .OrderBy(ge => ge.ShortDesc)
                .ToListAsync();

            return Ok(gameEvents);
        }

        [HttpPost("game/{gameId}/apply-event")]
        public async Task<IActionResult> ApplyEventToGame(int gameId, [FromBody] ApplyEventDto eventData)
        {
            var gameEvent = await _context.GameEvents.FindAsync(eventData.EventId);
            if (gameEvent == null)
            {
                return NotFound($"Nie znaleziono zdarzenia o ID {eventData.EventId}.");
            }

            // Znajdź wszystkie drużyny w danej grze
            var teamsInGame = await _context.Teams.Where(t => t.GameId == gameId).ToListAsync();
            if (!teamsInGame.Any())
            {
                // Możemy aktywować event nawet jeśli nie ma drużyn, log i tak się utworzy.
                // To zależy od logiki biznesowej. Załóżmy, że jest to OK.
            }

            // Zaktualizuj każdą drużynę
            foreach (var team in teamsInGame)
            {
                team.GameEventId = gameEvent.GameEventId;
                team.TurnsLeft = gameEvent.TurnTime;
            }

            // Stwórz wpis w GameLogs, aby poinformować o zdarzeniu.
            // Ponieważ pola są nullowalne, możemy je po prostu pominąć.
            var eventLog = new GameLog
            {
                Data = DateTime.UtcNow,
                GameId = gameId,
                GameEventId = gameEvent.GameEventId,
                IsApproved = true,
                // Pola takie jak TeamId, CardId, DeckId, BoardId, Cost, Status
                // zostaną automatycznie ustawione na NULL, bo ich tu nie podajemy.
            };
            _context.GameLogs.Add(eventLog);

            // Zapisz wszystkie zmiany w jednej transakcji
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group($"game-{gameId}").SendAsync("HistoryUpdated");

            return Ok(new { message = $"Zdarzenie '{gameEvent.EventShortDesc}' zostało aktywowane dla wszystkich drużyn." });
        }

        [HttpPost("player-history")]
        public async Task<IActionResult> GetPlayerHistory([FromBody] LogData logData)
        {
            // 1. Pobierz logi specyficzne dla drużyny (gdzie IsApproved != false)
            var teamLogsQuery = _context.GameLogs
                .Where(gl => gl.GameId == logData.GameId && gl.TeamId == logData.TeamId && gl.IsApproved != false);

            // 2. Pobierz globalne logi zdarzeń (gdzie TeamId jest null)
            var eventNotificationLogsQuery = _context.GameLogs
                .Where(gl => gl.GameId == logData.GameId && gl.TeamId == null);

            // 3. Połącz oba zapytania
            var combinedLogs = await teamLogsQuery
                .Union(eventNotificationLogsQuery)
                .OrderByDescending(gl => gl.Data)
                .Include(gl => gl.Team)
                .Include(gl => gl.Feedback)
                .Include(gl => gl.GameEvent) // Dołączamy, aby mieć opis zdarzenia
                .ToListAsync();

            if (!combinedLogs.Any()) return Ok(new List<object>());

            var cardIds = combinedLogs.Where(gl => gl.CardId.HasValue).Select(gl => gl.CardId.Value).Distinct().ToList();

            var decisions = await _context.Decisions.Where(d => cardIds.Contains(d.CardId)).ToListAsync();
            var decisionTitles = decisions.GroupBy(d => d.CardId).ToDictionary(g => g.Key, g => g.First().DecisionShortDesc);

            var items = await _context.Items.Where(i => cardIds.Contains(i.CardId)).ToListAsync();
            var itemTitles = items.GroupBy(i => i.CardId).ToDictionary(g => g.Key, g => g.First().HardwareShortDesc);

            var result = combinedLogs.Select(gl =>
            {
                // Sprawdzamy, czy to powiadomienie o evencie
                bool isEventNotification = gl.TeamId == null;

                return new
                {
                    IsEventNotification = isEventNotification,
                    Timestamp = gl.Data,
                    TeamName = gl.Team?.TeamName,
                    CardId = gl.CardId,
                    CardTitle = gl.CardId.HasValue ? (decisionTitles.GetValueOrDefault(gl.CardId.Value) ?? itemTitles.GetValueOrDefault(gl.CardId.Value)) : null,
                    FeedbackDescription = gl.Feedback?.LongDescription,
                    EventDescription = gl.GameEvent?.EventLongDesc, // Opis z dołączonego eventu
                    Status = gl.Status,
                    Cost = gl.Cost,
                    GameEventId = gl.GameEventId
                };
            });

            return Ok(result);
        }

        /// Zwraca "wersję" historii (zatwierdzonych logów) dla gry lub konkretnej drużyny.
        [HttpGet("game/{gameId}/history-version")]
        public async Task<IActionResult> GetHistoryVersion(int gameId, [FromQuery] int teamId = 0)
        {
            var query = _context.GameLogs.Where(gl => gl.GameId == gameId && gl.IsApproved != false);

            if (teamId > 0)
            {
                // Wersja dla gracza: jego logi + logi globalne (bez teamId)
                query = query.Where(gl => gl.TeamId == teamId || gl.TeamId == null);
            }

            var logCount = await query.CountAsync();
            var lastLogDate = await query.OrderByDescending(gl => gl.Data).Select(gl => gl.Data).FirstOrDefaultAsync();

            if (logCount == 0) return Ok(new { version = "empty" });

            return Ok(new { version = $"{logCount}-{lastLogDate.Ticks}" });
        }

        /// Zwraca "wersję" sugestii (oczekujących logów) dla całej gry.
        [HttpGet("game/{gameId}/pending-version")]
        public async Task<IActionResult> GetPendingLogsVersion(int gameId)
        {
            var query = _context.GameLogs.Where(gl => gl.GameId == gameId && gl.IsApproved == false);

            var logCount = await query.CountAsync();
            var lastLogDate = await query.OrderByDescending(gl => gl.Data).Select(gl => gl.Data).FirstOrDefaultAsync();

            if (logCount == 0) return Ok(new { version = "empty" });

            return Ok(new { version = $"{logCount}-{lastLogDate.Ticks}" });
        }

        private async Task AddGameLogSpecsForCard(GameLog gameLogEntry, GameEvent? activeEvent)
        {
            var cardId = gameLogEntry.CardId;
            var deckId = gameLogEntry.DeckId;
            var gameId = gameLogEntry.GameId;
            var teamId = gameLogEntry.TeamId.Value; // Wiemy, że tu nie będzie null

            // Krok 1: Pobierz wagi dla danej karty
            var decisionWeights = await _context.DecisionWeights
                .Where(dw => dw.CardId == cardId && dw.DeckId == deckId)
                .ToListAsync();

            if (!decisionWeights.Any()) return; // Jeśli nie ma wag, nie ma nic do roboty

            // --- KLUCZOWA POPRAWKA ---
            // Krok 2: Stwórz mapę tłumaczącą ProcessId -> GameProcessId dla tej gry i drużyny
            var processIdToGameProcessIdMap = await _context.GameProcesses
                .Where(gp => gp.GameId == gameId && gp.TeamId == teamId)
                .ToDictionaryAsync(
                    gp => gp.ProcessId,      // Klucz: ID ogólnego procesu (np. 5)
                    gp => gp.GameProcessId   // Wartość: ID instancji tego procesu w grze (np. 123)
                );

            var gameLogSpecs = new List<GameLogSpec>();

            foreach (var dw in decisionWeights)
            {
                // Krok 3: Znajdź odpowiedni GameProcessId dla ProcessId z wagi
                if (processIdToGameProcessIdMap.TryGetValue(dw.ProcessId, out var gameProcessId))
                {
                    // Mamy pasujący GameProcessId, możemy stworzyć poprawny wpis
                    var (finalMoveX, finalMoveY) = CalculateBoostedMoves(dw, activeEvent); // Używamy metody pomocniczej

                    gameLogSpecs.Add(new GameLogSpec
                    {
                        GameLogId = gameLogEntry.GameLogId,
                        GameProcessId = gameProcessId, // Używamy POPRAWNEGO ID
                        MoveX = finalMoveX,
                        MoveY = finalMoveY,
                    });
                }
            }
            // --- KONIEC POPRAWKI ---

            if (gameLogSpecs.Any())
            {
                _context.GameLogSpecs.AddRange(gameLogSpecs);
                // SaveChanges() jest w metodzie nadrzędnej, więc tutaj tylko dodajemy do kontekstu
            }

            // Wywołania do PlayerService zostają bez zmian
            await _playerService.SetGameProcessPosAsync(gameId, teamId);
            await _playerService.SetTeamPosAsync(gameId, teamId);
        }
        private (int MoveX, int MoveY) CalculateBoostedMoves(DecisionWeight weight, GameEvent? activeEvent)
        {
            // Zaczynamy od wartości bazowych z DecisionWeight
            double finalMoveX = weight.WeightX;
            double finalMoveY = weight.WeightY;

            // Jeśli nie ma aktywnego eventu, nie ma nic więcej do roboty
            if (activeEvent != null)
            {
                // Stosujemy modyfikatory, jeśli istnieją, używając drugiej funkcji pomocniczej
                finalMoveX = ApplyBooster(finalMoveX, activeEvent.BoosterX);
                finalMoveY = ApplyBooster(finalMoveY, activeEvent.BoosterY);
            }

            // Używamy Math.Round do jawnego zaokrąglenia do najbliższej liczby całkowitej,
            // co jest bezpieczniejsze i bardziej przewidywalne niż proste rzutowanie (int).
            return ((int)Math.Round(finalMoveX), (int)Math.Round(finalMoveY));
        }

        private double ApplyBooster(double baseValue, double? booster)
        {
            // Jeśli booster ma wartość (nie jest null), zastosuj go.
            // W przeciwnym razie, zwróć niezmienioną wartość bazową.
            // Przykład: baseValue=100, booster=0.2 -> 100 * (1 + 0.2) = 120
            return booster.HasValue ? baseValue * (1 + booster.Value) : baseValue;
        }
    }

}