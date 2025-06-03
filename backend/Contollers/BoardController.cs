 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BoardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Board/user/5
        [Authorize]        
        [HttpGet("get")]
        
       public async Task<IActionResult> GetBoardsForUser()
        {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        return Unauthorized();

        var boards = await _context.Boards
        .Where(b => b.UserId == null || b.UserId == userId)
        .ToListAsync();

        return Ok(boards);
}
    }
}