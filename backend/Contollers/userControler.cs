using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.PdfGeneration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.IO;
using Microsoft.CodeAnalysis;

namespace backend.Controllers
{

        public class BoardExportRequest
    {
        public int TeamBoardId { get; set; }
        public int RivalBoardId { get; set; }
    }
    [ApiController]
    [Route("api/admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        [Authorize]
        [HttpGet("licenses")]
        public async Task<IActionResult> GetUserLicenses()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Użytkownik nie istnieje." });

            // Przykładowe podzielenie użytych licencji:
            var gamesFinished = user.LicensesUsed;
            var gamesInSession = await _context.Games
                .Where(g => g.UserId == userId &&
                            (g.GameStatus == backend.Data.GameStatus.During ||
                             g.GameStatus == backend.Data.GameStatus.Paused))
                .CountAsync();

            return Ok(new
            {
                licensesOwned = user.LicensesOwned,
                licensesUsed = user.LicensesUsed,
                licensesLeft = user.LicensesOwned - user.LicensesUsed,
                gamesInSession = gamesInSession,
                gamesFinished = gamesFinished
            });
        }

        [HttpGet("exportCards")]
        public async Task<IActionResult> GenerateCardsPdf([FromQuery] int deckId)
        {
            // --- Krok 1: Pobieranie i przygotowanie danych (bez zmian) ---
            // ... twój kod pobierający dane i tworzący `sortedCards` ...

            var allCards = new List<CardPdfModel>();

            var decisionCards = await _context.Decisions
                .Where(d => d.DeckId == deckId)
                .Select(d => new CardPdfModel
                {
                    Id = d.CardId,
                    Title = d.DecisionShortDesc,
                    Description = d.DecisionLongDesc,
                    Application = "Zastosowanie dla Decyzji",
                    CardType = "Decision"
                })
                .ToListAsync();
            allCards.AddRange(decisionCards);

            var itemCards = await _context.Items
                .Where(i => i.DeckId == deckId)
                .Select(i => new CardPdfModel
                {
                    Id = i.CardId,
                    Title = i.HardwareShortDesc,
                    Description = i.HardwareLongDesc,
                    Application = "Zastosowanie dla Przedmiotu",
                    CardType = (i.CardId >= 100 && i.CardId < 200) ? "Software" : "Hardware"
                })
                .ToListAsync();
            allCards.AddRange(itemCards);

            var sortedCards = allCards.OrderBy(c => c.Id).ToList();

            if (!sortedCards.Any())
            {
                return NotFound($"Nie znaleziono żadnych kart dla talii o ID {deckId}.");
            }

            List<CardPdfModel> processedCards = ProcessCards(sortedCards);

            // --- Krok 2: Generowanie PDF (bez zmian) ---
            // Użyj ostatniej, uproszczonej wersji CardsDocument
            var document = new CardsDocument(processedCards);
            byte[] pdfBytes = document.GeneratePdf();

            // --- Krok 3: Zwróć plik do frontendu (bez zmian) ---
            // Ten kod wciąż działa, aby frontend nie zgłaszał błędu.
            var browserFileName = "DigitalWars_Karty.pdf";
            return File(pdfBytes, "application/pdf", browserFileName);
        }

        /// Generuje i zwraca plik PDF z wybranymi planszami.
        [HttpPost("exportBoards")]
        public async Task<IActionResult> GenerateBoardsPdf([FromBody] BoardExportRequest request)
        {
            // Walidacja - sprawdzamy, czy ID są większe od 0
            if (request.TeamBoardId <= 0 || request.RivalBoardId <= 0)
            {
                return BadRequest("Należy podać prawidłowe ID dla obu plansz.");
            }

            // Pobierz dane OBU plansz z bazy w jednym zapytaniu
            var boards = await _context.Boards
                .Where(b => b.BoardId == request.TeamBoardId || b.BoardId == request.RivalBoardId)
                .ToListAsync();

            // Sprawdzamy, czy znaleziono obie plansze
            if (boards.Count < 2)
            {
                return NotFound("Nie znaleziono jednej lub obu plansz o podanych ID.");
            }

            // Stwórz dokument QuestPDF, przekazując ID, aby wiedział, co gdzie umieścić
            var document = new BoardsDocument(boards, request.TeamBoardId, request.RivalBoardId);

            // Wygeneruj PDF do strumienia bajtów
            byte[] pdfBytes = document.GeneratePdf();

            var fileName = "DigitalWars_Plansze.pdf";

            // Zwróć plik
            return File(pdfBytes, "application/pdf", fileName);
        }

         public static List<CardPdfModel> ProcessCards(List<CardPdfModel> cards)
        {
            var processedCards = new List<CardPdfModel>();
            const string marker = "Zastosowanie:";

            foreach (var card in cards)
            {
                var processedCard = new CardPdfModel
                {
                    Id = card.Id,
                    Title = card.Title,
                    CardType = card.CardType,
                    Description = card.Description, // Domyślnie przypisujemy całość
                    Application = string.Empty      // Domyślnie puste
                };

                // Szukamy naszego znacznika w opisie
                int markerIndex = processedCard.Description.IndexOf(marker, System.StringComparison.OrdinalIgnoreCase);

                if (markerIndex != -1)
                {
                    // Znaleziono znacznik. Dzielimy tekst.
                    
                    // Application to tekst PO znaczniku
                    processedCard.Application = processedCard.Description.Substring(markerIndex + marker.Length).Trim();
                    
                    // Description to tekst PRZED znacznikiem
                    processedCard.Description = processedCard.Description.Substring(0, markerIndex).Trim();
                }

                processedCards.Add(processedCard);
            }

            return processedCards;
        }
    }
}
