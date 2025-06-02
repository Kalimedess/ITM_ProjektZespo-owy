using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckDownloadController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeckDownloadController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/deckdownload
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDecks()
        {
            var decks = await _context.Decks
                                      .Select(deck => new {
                                          id = deck.DeckId,
                                          title = deck.DeckName
                                      })
                                      .ToListAsync();

            return Ok(decks);
        }
    }
}