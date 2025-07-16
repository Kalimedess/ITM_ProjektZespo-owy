using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Threading.Tasks;
using System;

namespace backend.Controllers
{
    [Route("api/password")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PasswordController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Weryfikacja tokenu (GET)
        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if (user == null)
                return NotFound(new { valid = false });

            return Ok(new { valid = true });
        }

        // 2. Ustawienie nowego hasła (POST)
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Token and new password are required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == request.Token);
            if (user == null)
                return BadRequest("Invalid or expired token.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ConfirmationToken = null;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Password reset successful." });
        }
    }

    public class ResetPasswordWithTokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
