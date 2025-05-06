using backend.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using ClosedXML.Excel;

public class DecisionData
{
    public int CardId { get; set; }
    public string ShortDesc { get; set; } = string.Empty;
    public string LongDesc { get; set; } = string.Empty;
}

public class ItemsData
{
    public int CardId {get; set;}
    public string ShortDesc {get; set;} = string.Empty;
    public string LongDesc {get; set;} = string.Empty;
    public double BaseCost {get; set;}
}

public class FeedbacksData
{
    public int CardId {get; set;}
    public bool Status {get; set;}
    public string LongDesc {get; set;} = string.Empty;
    public string FeedbackPDF {get; set;} = string.Empty;
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


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nie przesłano żadnego pliku.");
            }

            //Sprawdzanie typu
            var allowedTypes = new[] {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // .xlsx
                "application/vnd.ms-excel"  // .xls
            };

            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest("Nieprawidłowy typ pliku. Akceptowane są tylko pliki Excel (.xlsx, .xls).");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var workbook = new XLWorkbook(stream);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "DigitalWars_SzablonKart.xlsx");

            using var templateWorkbook = new XLWorkbook(templatePath);    

            //Sprawdzanie nazw i ilosci arkuszy
            var templateSheetNames = templateWorkbook.Worksheets.Select(ws => ws.Name).ToList();
            var actualSheetNames = workbook.Worksheets.Select(ws => ws.Name).ToList();

            if(actualSheetNames.Count != templateSheetNames.Count)
            {
                return BadRequest("Plik musi zawierać dokładnie 3 arkusze: Decisions, Items, Feedbacks.");
            }

            for(int i=0; i<templateSheetNames.Count; i++)
            {
                if(actualSheetNames[i]!=templateSheetNames[i])
                {
                    return BadRequest($"Arkusz nr {i + 1} powinien mieć nazwę '{templateSheetNames[i]}', ale znaleziono '{actualSheetNames[i]}'.");                   
                }
            }

            //Sprawdzanie kolumn
            var templateSheetDecisions = templateWorkbook.Worksheet("Decisions");
            var templateSheetItems = templateWorkbook.Worksheet("Items");
            var templateSheetFeedbacks = templateWorkbook.Worksheet("Feedbacks");

            var templateDecisionsName = templateSheetDecisions.Row(1).Cells(1, 3).Select(c => c.GetString()).ToArray();
            var templateItemsName = templateSheetItems.Row(1).Cells(1, 4).Select(c => c.GetString()).ToArray();
            var templateFeedbacksName = templateSheetFeedbacks.Row(1).Cells(1, 4).Select(c => c.GetString()).ToArray();

            var sheetDecisions = workbook.Worksheet("Decisions");
            var sheetItems = workbook.Worksheet("Items");
            var sheetFeedbacks = workbook.Worksheet("Feedbacks");

            var decisionsNames = sheetDecisions.Row(1).Cells(1, 3).Select(c => c.GetString()).ToArray();
            var itemsNames = sheetItems.Row(1).Cells(1, 4).Select(c => c.GetString()).ToArray();
            var feedbacksNames = sheetFeedbacks.Row(1).Cells(1, 4).Select(c => c.GetString()).ToArray();

            if (!decisionsNames.SequenceEqual(templateDecisionsName))
            {
                return BadRequest($"Arkusz 'Decisions' powinien mieć kolumny: {string.Join(", ", templateDecisionsName)}.");
            }
            if (!itemsNames.SequenceEqual(templateItemsName))
            {
                return BadRequest($"Arkusz 'Items' powinien mieć kolumny: {string.Join(", ", templateItemsName)}.");
            }
            if (!feedbacksNames.SequenceEqual(templateFeedbacksName))
            {
                return BadRequest($"Arkusz 'Feedbacks' powinien mieć kolumny: {string.Join(", ", templateFeedbacksName)}.");
            }

            // Template Data Decisions
            var templateDecisionsData = templateSheetDecisions
            .Range("A2:C39")
            .Rows()
            .Select(row => new DecisionData
            {
                CardId = row.Cell(1).GetValue<int>(),
                ShortDesc = row.Cell(2).GetString(),
                LongDesc = row.Cell(3).GetString()
            })
            .ToList();

            // Template Data Items
            var templateItemsData = templateSheetItems
            .Range("A2:D36")
            .Rows()
            .Select(row => new ItemsData
            {
                CardId = row.Cell(1).GetValue<int>(),
                ShortDesc = row.Cell(2).GetString(),
                LongDesc = row.Cell(3).GetString(),
                BaseCost = row.Cell(4).GetValue<double>() 
            })
            .ToList();

            // Template Data Feedbacks
            var templateFeedbacksData = templateSheetFeedbacks
            .Range("A2:D66")
            .Rows()
            .Select(row => new FeedbacksData
            {
                CardId = row.Cell(1).GetValue<int>(),
                Status = bool.TryParse(row.Cell(2).GetString(), out bool status) ? status : false,
                LongDesc = row.Cell(3).GetString(),
                FeedbackPDF = row.Cell(4).GetString()
            })
            .ToList();    


            // Decisions: kolumny A–C, wiersze 2–39
            var decisionsData = sheetDecisions
            .Range("A2:C39")
            .Rows()
            .Select(row => new DecisionData
            {
                CardId = row.Cell(1).GetValue<int>(),
                ShortDesc = row.Cell(2).GetString(),
                LongDesc = row.Cell(3).GetString()
            })
            .ToList();

            // Items: kolumny A-D, wiersze 2-36
            var itemsData = sheetItems
            .Range("A2:D36")
            .Rows()
            .Select(row => new ItemsData
            {
                CardId = row.Cell(1).GetValue<int>(),
                ShortDesc = row.Cell(2).GetString(),
                LongDesc = row.Cell(3).GetString(),
                BaseCost = row.Cell(4).GetValue<double>() 
            })
            .ToList();

            // Feedbacks: kolumny A-D, wiersze 2-66
            var feedbacksData = sheetFeedbacks
            .Range("A2:D66")
            .Rows()
            .Select(row => new FeedbacksData
            {
                CardId = row.Cell(1).GetValue<int>(),
                Status = bool.TryParse(row.Cell(2).GetString(), out bool status) ? status : false,
                LongDesc = row.Cell(3).GetString(),
                FeedbackPDF = row.Cell(4).GetString()
            })
            .ToList();


            // Sprawdzanie niezmienianych danych w Decisions
            for(int i=0; i<decisionsData.Count;i++)
            {
                var decisions = decisionsData[i];
                var templateDecisions = templateDecisionsData[i];

                if(decisions.CardId != templateDecisions.CardId)
                {
                    return BadRequest($"Decisions: Mismatch at index {i + 1}: uploaded CardId = {decisions.CardId}, template CardId = {templateDecisions.CardId}");
                }
            }
            
            // Sprawdzanie niezmienianych danych w Items
            for(int i=0; i<itemsData.Count;i++)
            {
                var items = itemsData[i];
                var templateItems = templateItemsData[i];
                
                if(items.CardId != templateItems.CardId || items.BaseCost != templateItems.BaseCost)
                {
                    return BadRequest($"Items: Mismatch at index {i + 1}: uploaded CardId = {items.CardId}, template CardId = {templateItems.CardId}, uploaded BaseCost = {items.BaseCost}, template BaseCost = {templateItems.BaseCost}");
                }
            }

            // Sprawdzanie niezmienianych danych w Feedbacks
            for(int i=0;i<feedbacksData.Count;i++)
            {
                var feedbacks = feedbacksData[i];
                var templateFeedbacks = templateFeedbacksData[i];

                if(feedbacks.CardId != templateFeedbacks.CardId || feedbacks.Status != templateFeedbacks.Status)
                {
                    return BadRequest($"Feedbacks: Mismatch at index {i + 1}: uploaded CardId = {feedbacks.CardId}, template CardId = {templateFeedbacks.CardId}, uploaded Status = {feedbacks.Status}, template Status = {templateFeedbacks.Status}");
                }
            }

            int maxDeckId = await _context.Decks.MaxAsync(d => (int?)d.DeckId) ?? 0;
            int newDeckId = maxDeckId + 1;

            return Ok(new {deckId = newDeckId, dec = decisionsData, ite = itemsData, fed = feedbacksData });
        }

    }
}
