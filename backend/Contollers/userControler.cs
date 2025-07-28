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
        
        [HttpGet("enablersMap")]
        public async Task<IActionResult> GetEnablersMap()
        {
            var enablersMap = await _context.Cards
                .Include(card => card.DecisionEnablers
                                     // Od razu filtruj dołączane dane, to bardzo wydajne
                                     .Where(de => de.EnablerId.HasValue))

                // 2. Teraz filtruj same karty
                .Where(card => card.CardType == CardType.Decision)

                // 3. Konwersja do słownika
                .ToDictionaryAsync(
                    card => card.CardId,
                    card => card.DecisionEnablers.Select(de => de.EnablerId.Value).ToList()
                );

            return Ok(enablersMap);
        }

        [HttpGet("latestEntries")]
        public async Task<IActionResult> GetLatestEntries(int gameId, int? teamId = null)
        {
            // Scenariusz 1: Podano gameId i teamId (bez zmian)
            // ------------------------------------------------
            if (teamId.HasValue)
            {
                var latestCardId = await _context.GameLogs
                    .Where(gl => gl.GameId == gameId && gl.TeamId == teamId.Value && gl.Status == true)
                    .OrderByDescending(gl => gl.Data)
                    .Select(gl => gl.CardId)
                    .FirstOrDefaultAsync();

                if (latestCardId == 0)
                {
                    return NotFound($"Nie znaleziono zagranych kart dla GameId: {gameId} i TeamId: {teamId.Value}.");
                }

                return Ok(latestCardId);
            }
            // Scenariusz 2: Podano tylko gameId (zmiana tutaj)
            // ------------------------------------------------
            else
            {
                var latestEntriesByTeam = await _context.GameLogs
                    .Where(gl => gl.GameId == gameId && gl.Status == true)
                    .GroupBy(gl => gl.TeamId)
                    // Zamiast DTO, tworzymy obiekt anonimowy.
                    // Nazwy właściwości (TeamId, CardId) zostaną użyte jako klucze w JSON-ie.
                    .Select(group => new
                    {
                        TeamId = group.Key,
                        TeamColor = group.First().Team.TeamColor,
                        CardId = group.OrderByDescending(g => g.Data).First().CardId
                    })
                    .ToListAsync();

                return Ok(latestEntriesByTeam);
            }
        }
    }
}
