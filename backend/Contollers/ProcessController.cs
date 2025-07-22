using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProcessesDto
{
    public int ProcessId { get; set; }

    public string ProcessDesc { get; set; } = string.Empty; // Odpowiada 'shortName'
    public string ProcessLongDesc { get; set; } = string.Empty; // Odpowiada 'fullName'
    public string ProcessColor { get; set; } = string.Empty; // Odpowiada 'fullName'
}

namespace backend.Controllers
{
    [Authorize]
    [Route("api/processes")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProcessesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("by-deck/{deckId}")]
        [ProducesResponseType(typeof(IEnumerable<ProcessDto>), 200)] // Dla dokumentacji Swagger
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProcessesByDeckId(int deckId)
        {
            if (deckId <= 0)
            {
                return BadRequest("Nieprawidłowy identyfikator talii.");
            }

            // Sprawdźmy, czy talia o podanym ID w ogóle istnieje
            var deckExists = await _context.Decks.AnyAsync(d => d.DeckId == deckId);
            if (!deckExists)
            {
                return NotFound($"Nie znaleziono talii o ID: {deckId}");
            }

            // Pobierz procesy i od razu zmapuj je na DTO.
            // To bardziej wydajne, bo EF Core pobierze z bazy tylko potrzebne kolumny.
            var processes = await _context.Processes
                .Where(p => p.DeckId == deckId)
                .Select(p => new ProcessesDto
                {
                    ProcessId = p.ProcessId,
                    ProcessDesc = p.ProcessDesc,
                    ProcessLongDesc = p.ProcessLongDesc,
                    ProcessColor = p.ProcessColor
                })
                .ToListAsync();
            
            // Zwrócenie listy (nawet jeśli jest pusta) z kodem 200 OK jest standardową praktyką REST.
            return Ok(processes);
        }
    }
}