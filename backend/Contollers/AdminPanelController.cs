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
    [HttpGet("teams")] 
    public async Task<ActionResult<IEnumerable<object>>> GetTeams()
    {
        var teams = await _context.Teams
            .Select(team => new {
                id = team.TeamId,
                name = team.TeamName,
                color = team.TeamColor
            })
            .ToListAsync();

        return Ok(teams);
    }
}