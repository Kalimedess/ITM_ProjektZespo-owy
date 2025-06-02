using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class DecisionData
{
    public int CardId { get; set; }
    public string ShortDesc { get; set; } = string.Empty;
    public string LongDesc { get; set; } = string.Empty;
}

public class ItemsData
{
    public int CardId { get; set; }
    public string ShortDesc { get; set; } = string.Empty;
    public string LongDesc { get; set; } = string.Empty;
    public double BaseCost { get; set; }
}

public class FeedbacksData
{
    public int CardId { get; set; }
    public bool Status { get; set; }
    public string LongDesc { get; set; } = string.Empty;
    public string FeedbackPDF { get; set; } = string.Empty;
}

namespace backend.Controllers
{
    [Route("api/deck")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DeckController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // 1. Podstawowa walidacja pliku
            var validationError = ValidateUploadedFile(file);
            if (validationError != null) return validationError;

            // 2. Otwieranie przesłanego workbooka
            XLWorkbook? uploadedWorkbook;
            IActionResult? openUploadedError;
            (uploadedWorkbook, openUploadedError) = await OpenUploadedWorkbookAsync(file);
            if (openUploadedError != null) return openUploadedError;
            if (uploadedWorkbook == null) return StatusCode(StatusCodes.Status500InternalServerError, "Nie udało się otworzyć przesłanego pliku workbook (null).");

            using (uploadedWorkbook)
            {
                // 3. Otwieranie workbooka szablonu
                XLWorkbook? templateWorkbook;
                IActionResult? openTemplateError;
                (templateWorkbook, openTemplateError) = OpenTemplateWorkbook();
                if (openTemplateError != null) return openTemplateError;
                if (templateWorkbook == null) return StatusCode(StatusCodes.Status500InternalServerError, "Nie udało się otworzyć pliku szablonu workbook (null).");

                using (templateWorkbook)
                {
                    // 4. Walidacja struktury workbooka (arkusze, kolumny)
                    var structureError = ValidateWorkbookStructure(uploadedWorkbook, templateWorkbook);
                    if (structureError != null) return structureError;

                    // 5. Ekstrakcja danych z obu workbooków
                    var (uploadedDecisions, uploadedItems, uploadedFeedbacks) = ExtractDataFromWorkbook(uploadedWorkbook);
                    var (templateDecisions, templateItems, templateFeedbacks) = ExtractDataFromWorkbook(templateWorkbook);

                    // 6. Walidacja niezmiennych danych
                    var immutableDataError = ValidateImmutableData(
                        uploadedDecisions, templateDecisions,
                        uploadedItems, templateItems,
                        uploadedFeedbacks, templateFeedbacks);
                    if (immutableDataError != null) return immutableDataError;

                   await using var dbTransaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        // 7. Tworzenie i zapisywanie nowego Deck
                        int newDeckIdValue;
                        IActionResult? deckCreationError;
                        (var nullableDeckId, deckCreationError) = await CreateAndSaveNewDeckAsync(User);

                        if (deckCreationError != null)
                        {
                            await dbTransaction.RollbackAsync();
                            return deckCreationError;
                        }
                        if (!nullableDeckId.HasValue)
                        {
                            await dbTransaction.RollbackAsync();
                            return StatusCode(StatusCodes.Status500InternalServerError, "Nie udało się utworzyć nowej talii (brak DeckId).");
                        }
                        newDeckIdValue = nullableDeckId.Value;

                        // 8. Zapisywanie danych kart (Decisions, Items, Feedbacks)
                        List<Feedback> feedbacksWithPdfReport;
                        IActionResult? cardDataSaveError;
                        (feedbacksWithPdfReport, cardDataSaveError) = await SaveCardDataAsync(
                            newDeckIdValue,
                            uploadedDecisions,
                            uploadedItems,
                            uploadedFeedbacks);

                        if (cardDataSaveError != null)
                        {
                            await dbTransaction.RollbackAsync();
                            return cardDataSaveError;
                        }

                        await dbTransaction.CommitAsync();

                        // 9. Zwrócenie sukcesu
                        return Ok(new { message = "Dane zapisane pomyślnie", fed = feedbacksWithPdfReport });
                    }
                    catch (Exception ex)
                    {
                        await dbTransaction.RollbackAsync();
                        return StatusCode(StatusCodes.Status500InternalServerError, $"Wystąpił krytyczny błąd serwera podczas zapisu danych: {ex.Message}. Zmiany zostały wycofane.");
                    }
                }
            }
        }

        private IActionResult? ValidateUploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nie przesłano żadnego pliku.");
            }

            var allowedTypes = new[] {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // .xlsx
                "application/vnd.ms-excel"  // .xls
            };

            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest("Nieprawidłowy typ pliku. Akceptowane są tylko pliki Excel (.xlsx, .xls).");
            }
            return null;
        }

        private async Task<(XLWorkbook? workbook, IActionResult? errorResult)> OpenUploadedWorkbookAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            try
            {
                await file.CopyToAsync(stream);
                stream.Position = 0; // Resetowanie pozycji strumienia jest kluczowe!
                var workbook = new XLWorkbook(stream);
                return (workbook, null);
            }
            catch (Exception ex)
            {
                // _logger?.LogError(ex, "Error opening uploaded Excel file.");
                return (null, StatusCode(StatusCodes.Status500InternalServerError, $"Wystąpił błąd podczas otwierania przesłanego pliku Excel: {ex.Message}"));
            }
        }

        private (XLWorkbook? workbook, IActionResult? errorResult) OpenTemplateWorkbook()
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "DigitalWars_SzablonKart.xlsx");
            try
            {
                using var templateStream = System.IO.File.OpenRead(templatePath);
                var workbook = new XLWorkbook(templateStream);
                return (workbook, null);
            }
            catch (FileNotFoundException ex)
            {
                // _logger?.LogError(ex, "Template file not found.");
                return (null, StatusCode(StatusCodes.Status500InternalServerError, $"Nie znaleziono pliku szablonu '{Path.GetFileName(templatePath)}'. Szczegóły: {ex.Message}"));
            }
            catch (Exception ex)
            {
                // _logger?.LogError(ex, "Error opening template Excel file.");
                return (null, StatusCode(StatusCodes.Status500InternalServerError, $"Wystąpił błąd podczas otwierania pliku szablonu: {ex.Message}"));
            }
        }

        private IActionResult? ValidateWorkbookStructure(XLWorkbook uploadedWorkbook, XLWorkbook templateWorkbook)
        {
            var templateSheetNames = templateWorkbook.Worksheets.Select(ws => ws.Name).ToList();
            var actualSheetNames = uploadedWorkbook.Worksheets.Select(ws => ws.Name).ToList();

            if (actualSheetNames.Count != templateSheetNames.Count)
            {
                if (templateSheetNames.Count == 3 && templateSheetNames.All(name => name == "Decisions" || name == "Items" || name == "Feedbacks"))
                {
                     return BadRequest("Plik musi zawierać dokładnie 3 arkusze: Decisions, Items, Feedbacks.");
                }
                return BadRequest($"Niezgodna liczba arkuszy. Oczekiwano {templateSheetNames.Count}, otrzymano {actualSheetNames.Count}.");
            }

            for (int i = 0; i < templateSheetNames.Count; i++)
            {
                if (actualSheetNames[i] != templateSheetNames[i])
                {
                    return BadRequest($"Arkusz nr {i + 1} powinien mieć nazwę '{templateSheetNames[i]}', ale znaleziono '{actualSheetNames[i]}'.");
                }
            }

            var sheetConfigs = new[]
            {
                new { Name = "Decisions", Columns = 3 },
                new { Name = "Items", Columns = 4 },
                new { Name = "Feedbacks", Columns = 4 }
            };

            foreach (var config in sheetConfigs)
            {
                var templateSheet = templateWorkbook.Worksheet(config.Name);
                var uploadedSheet = uploadedWorkbook.Worksheet(config.Name);

                if (templateSheet == null || uploadedSheet == null) // Powinno być obsłużone przez walidację nazw arkuszy
                    return BadRequest($"Brak wymaganego arkusza: {config.Name}");

                var templateHeaders = templateSheet.Row(1).Cells(1, config.Columns).Select(c => c.GetString()).ToArray();
                var uploadedHeaders = uploadedSheet.Row(1).Cells(1, config.Columns).Select(c => c.GetString()).ToArray();

                if (!uploadedHeaders.SequenceEqual(templateHeaders))
                {
                    return BadRequest($"Arkusz '{config.Name}' powinien mieć kolumny: {string.Join(", ", templateHeaders)}. Znaleziono: {string.Join(", ", uploadedHeaders)}");
                }
            }
            return null;
        }

        private (List<DecisionData> decisions, List<ItemsData> items, List<FeedbacksData> feedbacks) ExtractDataFromWorkbook(XLWorkbook workbook)
        {
            var sheetDecisions = workbook.Worksheet("Decisions");
            var decisionsData = sheetDecisions.Range("A2:C39").Rows() // Zakładamy, że zakres jest stały i zawsze zawiera dane lub jest pusty
                .Select(row => new DecisionData
                {
                    CardId = row.Cell(1).GetValue<int>(),
                    ShortDesc = row.Cell(2).GetString(),
                    LongDesc = row.Cell(3).GetString()
                }).ToList();

            var sheetItems = workbook.Worksheet("Items");
            var itemsData = sheetItems.Range("A2:D36").Rows()
                .Select(row => new ItemsData
                {
                    CardId = row.Cell(1).GetValue<int>(),
                    ShortDesc = row.Cell(2).GetString(),
                    LongDesc = row.Cell(3).GetString(),
                    BaseCost = row.Cell(4).GetValue<double>()
                }).ToList();

            var sheetFeedbacks = workbook.Worksheet("Feedbacks");
            var feedbacksData = sheetFeedbacks.Range("A2:D66").Rows()
                .Select(row => new FeedbacksData
                {
                    CardId = row.Cell(1).GetValue<int>(),
                    Status = bool.TryParse(row.Cell(2).GetString(), out bool statusVal) ? statusVal : false,
                    LongDesc = row.Cell(3).GetString(),
                    FeedbackPDF = row.Cell(4).GetString()
                }).ToList();

            return (decisionsData, itemsData, feedbacksData);
        }

        private IActionResult? ValidateImmutableData(
            List<DecisionData> uploadedDecisions, List<DecisionData> templateDecisions,
            List<ItemsData> uploadedItems, List<ItemsData> templateItems,
            List<FeedbacksData> uploadedFeedbacks, List<FeedbacksData> templateFeedbacks)
        {
            // Sprawdzanie, czy liczby wierszy się zgadzają (dodatkowa asekuracja)
            if (uploadedDecisions.Count != templateDecisions.Count) return new BadRequestObjectResult("Niezgodna liczba wierszy w danych 'Decisions'.");
            if (uploadedItems.Count != templateItems.Count) return new BadRequestObjectResult("Niezgodna liczba wierszy w danych 'Items'.");
            if (uploadedFeedbacks.Count != templateFeedbacks.Count) return new BadRequestObjectResult("Niezgodna liczba wierszy w danych 'Feedbacks'.");


            for (int i = 0; i < uploadedDecisions.Count; i++)
            {
                if (uploadedDecisions[i].CardId != templateDecisions[i].CardId)
                    return new BadRequestObjectResult($"Decisions: Mismatch at index {i + 1}: uploaded CardId = {uploadedDecisions[i].CardId}, template CardId = {templateDecisions[i].CardId}");
            }

            for (int i = 0; i < uploadedItems.Count; i++)
            {
                if (uploadedItems[i].CardId != templateItems[i].CardId || uploadedItems[i].BaseCost != templateItems[i].BaseCost)
                    return new BadRequestObjectResult($"Items: Mismatch at index {i + 1}: uploaded CardId = {uploadedItems[i].CardId}, template CardId = {templateItems[i].CardId}, uploaded BaseCost = {uploadedItems[i].BaseCost}, template BaseCost = {templateItems[i].BaseCost}");
            }

            for (int i = 0; i < uploadedFeedbacks.Count; i++)
            {
                if (uploadedFeedbacks[i].CardId != templateFeedbacks[i].CardId || uploadedFeedbacks[i].Status != templateFeedbacks[i].Status)
                    return new BadRequestObjectResult($"Feedbacks: Mismatch at index {i + 1}: uploaded CardId = {uploadedFeedbacks[i].CardId}, template CardId = {templateFeedbacks[i].CardId}, uploaded Status = {uploadedFeedbacks[i].Status}, template Status = {templateFeedbacks[i].Status}");
            }
            return null;
        }

        private async Task<(int? newDeckId, IActionResult? errorResult)> CreateAndSaveNewDeckAsync(ClaimsPrincipal userPrincipal)
        {
            var userIdString = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = userPrincipal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userIdString) || string.IsNullOrEmpty(userName))
                return (null, new UnauthorizedObjectResult("Brak danych użytkownika."));

            if (!int.TryParse(userIdString, out int userId))
                return (null, new BadRequestObjectResult("Nieprawidłowy identyfikator użytkownika."));

            try
            {
                int maxDeckId = await _context.Decks.AnyAsync() ? await _context.Decks.MaxAsync(d => d.DeckId) : 0;
                int newDeckId = maxDeckId + 1;
                int userDeckCount = await _context.Decks.CountAsync(d => d.UserId == userId);
                int deckNumber = userDeckCount + 1;

                var deck = new Deck
                {
                    DeckId = newDeckId,
                    DeckName = $"Talia{userName}Nr{deckNumber}",
                    UserId = userId
                };
                _context.Decks.Add(deck);
                await _context.SaveChangesAsync();
                return (newDeckId, null);
            }
            catch (DbUpdateException ex)
            {
                // _logger?.LogError(ex, "DB update error while creating deck.");
                return (null, new ObjectResult($"Wystąpił błąd podczas zapisywania talii do bazy danych: {ex.InnerException?.Message ?? ex.Message}") { StatusCode = StatusCodes.Status500InternalServerError });
            }
            catch (Exception ex) // Ogólny błąd
            {
                // _logger?.LogError(ex, "Unexpected error while creating deck.");
                return (null, new ObjectResult($"Nieoczekiwany błąd podczas tworzenia talii: {ex.Message}") { StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        private async Task<(List<Feedback> feedbacksWithPdf, IActionResult? errorResult)> SaveCardDataAsync(
            int deckId,
            List<DecisionData> uploadedDecisions,
            List<ItemsData> uploadedItems,
            List<FeedbacksData> uploadedFeedbacks)
        {
            var feedbacksThatHadPdfData = new List<Feedback>();
            try
            {
                foreach (var data in uploadedDecisions)
                {
                    _context.Decisions.Add(new Decision { CardId = data.CardId, DeckId = deckId, DecisionShortDesc = data.ShortDesc, DecisionLongDesc = data.LongDesc });
                }
                await _context.SaveChangesAsync();

                foreach (var data in uploadedItems)
                {
                    _context.Items.Add(new Item { CardId = data.CardId, DeckId = deckId, HardwareShortDesc = data.ShortDesc, HardwareLongDesc = data.LongDesc });
                }
                await _context.SaveChangesAsync();

                foreach (var data in uploadedFeedbacks)
                {
                    var feedbackEntity = new Feedback { CardId = data.CardId, DeckId = deckId, Status = data.Status, LongDescription = data.LongDesc };
                    _context.Feedbacks.Add(feedbackEntity);
                    if (!string.IsNullOrWhiteSpace(data.FeedbackPDF))
                    {
                        feedbacksThatHadPdfData.Add(feedbackEntity);
                    }
                }
                await _context.SaveChangesAsync();
                return (feedbacksThatHadPdfData, null);
            }
            catch (DbUpdateException ex)
            {
                // _logger?.LogError(ex, "DB update error while saving card data.");
                return (new List<Feedback>(), new ObjectResult($"Wystąpił błąd podczas zapisywania danych kart do bazy danych: {ex.InnerException?.Message ?? ex.Message}") { StatusCode = StatusCodes.Status500InternalServerError });
            }
            catch (Exception ex) // Ogólny błąd
            {
                // _logger?.LogError(ex, "Unexpected error while saving card data.");
                return (new List<Feedback>(), new ObjectResult($"Nieoczekiwany błąd podczas zapisywania danych kart: {ex.Message}") { StatusCode = StatusCodes.Status500InternalServerError });
            }


    


        }


        [Authorize]
        [HttpGet("edit")]
        public async Task<ActionResult<IEnumerable<object>>> GetDecks()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = 0;

            if (!int.TryParse(userIdString, out userId))
            {
                return Unauthorized("Nieprawidłowy identyfikator użytkownika.");
            }

            var decks = await _context.Decks
                          .Where(deck => deck.UserId == null || deck.UserId == userId)
                          .Select(deck => new {
                              id = deck.DeckId,
                              title = deck.DeckName
                          })
                          .ToListAsync();

            return Ok(decks);
        }

        [Authorize]
        [HttpGet("decisions")]
        public async Task<ActionResult<IEnumerable<object>>> GetDecisionCards(int deckId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Nieprawidłowy identyfikator użytkownika.");
            }

            var deck = await _context.Decks.FirstOrDefaultAsync(d => d.DeckId == deckId && (d.UserId == null || d.UserId == userId));
            if (deck == null)
            {
                return NotFound("Talia nie istnieje lub brak dostępu.");
            }

            var decisionCards = await _context.Decisions
                .Where(decision => decision.DeckId == deckId)
                .Select(decision => new {
                    id = decision.CardId,
                    deckId= decision.DeckId,
                    title = decision.DecisionShortDesc,
                    description = decision.DecisionLongDesc,
                })
                .ToListAsync();

            return Ok(decisionCards);
        }

        


    }
}