using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data; 


[ApiController]
[Route("api/[controller]")]
public class AdminPanelController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminPanelController(AppDbContext context)
    {
        _context = context;
    }

    
    [Authorize]
    [HttpGet("teams/by-game/{gameId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetTeamsByGame(int gameId)
    {
        var teams = await _context.Teams
            .Where(t => t.GameId == gameId)
            .Select(t => new
            {
                id = t.TeamId,
                name = t.TeamName,
                color = t.TeamColor,
                token = t.TeamToken
            })
            .ToListAsync();

        return Ok(teams);
    }

    [Authorize]
    [HttpGet("games/{gameId}")]
    public async Task<ActionResult<object>> GetGameById(int gameId)
    {
        var game = await _context.Games
            .Where(g => g.GameId == gameId)
            .Select(g => new
            {
                id = g.GameId,
            })
            .FirstOrDefaultAsync();

    if (game == null)
    {
        return NotFound(new { message = "Gra nie została znaleziona." });
    }

    return Ok(game);
    }
}