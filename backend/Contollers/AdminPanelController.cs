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
                color = t.TeamColor
            })
            .ToListAsync();

        return Ok(teams);
    }
}