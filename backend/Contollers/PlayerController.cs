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
                // Filtr dla panelu "Do zatwierdzenia"
                query = query.Where(gl => gl.IsApproved == false);
            }
            else
            {
                // Filtr dla panelu "Historia decyzji" - pokazuj zatwierdzone i niezależne
                query = query.Where(gl => gl.IsApproved != false);
            }

            var gameLogs = await query.OrderByDescending(gl => gl.Data).ToListAsync();

            if (!gameLogs.Any()) return Ok(new List<object>());

            var cardIds = gameLogs.Select(gl => gl.CardId).Distinct().ToList();

            var decisions = await _context.Decisions.Where(d => cardIds.Contains(d.CardId)).ToListAsync();
            var decisionTitles = decisions.GroupBy(d => d.CardId).ToDictionary(g => g.Key, g => g.First().DecisionShortDesc);

            var items = await _context.Items.Where(i => cardIds.Contains(i.CardId)).ToListAsync();
            var itemTitles = items.GroupBy(i => i.CardId).ToDictionary(g => g.Key, g => g.First().HardwareShortDesc);

            var result = gameLogs.Select(gl => new
            {
                LogId = gl.GameLogId,
                Timestamp = gl.Data,
                TeamId = gl.TeamId,
                TeamName = gl.Team?.TeamName,
                CardId = gl.CardId,
                CardTitle = decisionTitles.GetValueOrDefault(gl.CardId) ?? itemTitles.GetValueOrDefault(gl.CardId) ?? "Nieznana karta",
                FeedbackDescription = gl.Feedback?.LongDescription,
                Status = gl.Status,
                Cost = gl.Cost,
                MoveX = gl.MoveX,
                MoveY = gl.MoveY
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

        private async Task<IActionResult> ProcessCardPlay(int selectedCard, bool wasSuccess, CardDataDTO data)
        {
            var team = await _context.Teams.FindAsync(data.TeamId);
            if (team == null) return NotFound($"Drużyna o ID {data.TeamId} nie została znaleziona.");

            // Logika finalStatus pozostaje taka sama, ale bez usuwania enablera tutaj
            var isItem = await _context.Items.AnyAsync(i => i.CardId == selectedCard);
            bool finalStatus = isItem || wasSuccess;

            var specialEnablerUsed = await _context.DecisionEnablers.AnyAsync(de =>
                de.CardId == selectedCard && de.GameId == data.GameId && de.TeamId == data.TeamId && de.EnablerId == null);

            if (specialEnablerUsed)
            {
                finalStatus = true;
            }

            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f =>
                f.CardId == selectedCard && f.DeckId == data.DeckId && f.Status == finalStatus);

            // Tworzymy log, ale go jeszcze nie zapisujemy
            var gameLogEntry = new GameLog
            {
                Data = DateTime.UtcNow,
                TeamId = data.TeamId,
                GameId = data.GameId,
                CardId = selectedCard,
                DeckId = data.DeckId,
                BoardId = data.BoardId,
                GameProcessId = data.GameProcessId,
                FeedbackId = feedback?.FeedbackId,
                Cost = data.Cost,
                Status = finalStatus,
                MoveX = 0,
                MoveY = 0,
                // Ustawiamy IsApproved na podstawie flagi drużyny
                IsApproved = data.ForceExecution ? (bool?)null : (team.IsIndependent ? (bool?)null : false)
            };
            _context.GameLogs.Add(gameLogEntry);

            // Jeśli drużyna jest niezależna i administrator zagrywa kartę, od razu wykonaj efekty
            if (team.IsIndependent || data.ForceExecution)
            {
                await ExecuteCardEffects(gameLogEntry);
            }

            // Zapisujemy wszystkie zmiany (log i potencjalne efekty) w jednej transakcji
            await _context.SaveChangesAsync();

            // Zwracamy odpowiedź do frontendu
            return Ok(new
            {
                message = team.IsIndependent ? "Akcja karty została wykonana." : "Sugestia zagrania karty została wysłana do zatwierdzenia.",
                newTeamBudget = team.TeamBud // Zawsze wysyłamy aktualny budżet
            });
        }

        private async Task ExecuteCardEffects(GameLog log)
        {
            // Ta metoda wykonuje wszystkie "fizyczne" zmiany w grze
            var team = await _context.Teams
                .AsNoTracking() // Używamy AsNoTracking, bo nie zamierzamy modyfikować samej drużyny w tej metodzie
                .FirstOrDefaultAsync(t => t.TeamId == log.TeamId);

            if (team == null) return; // Zabezpieczenie

            // Pobieramy drużynę jeszcze raz, ale tym razem do śledzenia zmian w budżecie
            var trackedTeam = await _context.Teams.FindAsync(log.TeamId);
            if(trackedTeam != null)
            {
                // 1. Odejmij bity
                trackedTeam.TeamBud -= (int)log.Cost;
            }

            // --- KLUCZOWA POPRAWKA ---
            // 2. Zmień status IsApproved z 'false' na 'true' TYLKO jeśli log był sugestią.
            // Jeśli był 'null' (dla gracza niezależnego), zostaw go jako 'null'.
            if (log.IsApproved == false)
            {
                log.IsApproved = true;
            }
            // --- KONIEC POPRAWKI ---

            // 3. Sprawdź, czy trzeba usunąć specjalny enabler (jeśli taki był użyty)
            var specialEnabler = await _context.DecisionEnablers.FirstOrDefaultAsync(de =>
                de.CardId == log.CardId &&
                de.GameId == log.GameId &&
                de.TeamId == log.TeamId &&
                de.EnablerId == null);
            
            if (specialEnabler != null)
            {
                _context.DecisionEnablers.Remove(specialEnabler);
            }

            // TODO: W przyszłości dodaj tutaj inne efekty, np. ruch pionka
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
                    DeckId = deckId
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

            return Ok(new { message = "Sugestia została odrzucona." });
        }
    }
}