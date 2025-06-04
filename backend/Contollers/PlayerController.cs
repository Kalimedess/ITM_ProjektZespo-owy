using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class UnifiedCardDto
{
    public int Id { get; set; }
    public int DisplayOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
}

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("deck/{deckId}/unified-cards")]
        public async Task<ActionResult<IEnumerable<UnifiedCardDto>>> GetUnifiedCardsForDeck(int deckId)
        {
            var deckExists = await _context.Decks.AnyAsync(d => d.DeckId == deckId);
            if (!deckExists)
            {
                return NotFound($"Talia o ID {deckId} nie została znaleziona.");
            }

            var decisionCardsInfo = await _context.Decisions
                .Where(d => d.DeckId == deckId)
                .Select(d => new
                {
                    d.CardId,
                    Title = d.DecisionShortDesc,
                    Description = d.DecisionLongDesc,
                    Type = "Decision"
                })
                .ToListAsync();

            var itemCardsInfo = await _context.Items
                .Where(i => i.DeckId == deckId)
                .Select(i => new
                {
                    i.CardId,
                    Title = i.HardwareShortDesc,
                    Description = i.HardwareLongDesc,
                    Type = "Item"
                })
                .ToListAsync();


            var allCardsTemp = decisionCardsInfo
                .Concat(itemCardsInfo)
                .OrderBy(c => c.CardId)
                .ToList();

            var unifiedCards = allCardsTemp
                .Select((c, index) => new UnifiedCardDto
                {
                    Id = c.CardId,
                    DisplayOrder = index + 1,
                    Title = c.Title,
                    Description = c.Description,
                    CardType = c.Type
                })
                .ToList();

            return Ok(unifiedCards);
        }
    }
}