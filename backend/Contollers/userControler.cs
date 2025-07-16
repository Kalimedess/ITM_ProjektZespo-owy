using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
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
    }
}
