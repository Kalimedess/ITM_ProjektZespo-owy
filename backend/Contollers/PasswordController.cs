using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Threading.Tasks;
using System;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/password")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public PasswordController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // 1. Zabezpieczenie przed "user enumeration"
            // Zawsze zwracamy OK, nawet jeśli użytkownik nie istnieje.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user != null)
            {
                // Generujemy token i zapisujemy go w bazie TYLKO jeśli użytkownik istnieje
                var resetPasswordToken = Guid.NewGuid().ToString();
                user.LinkToken = resetPasswordToken;
                user.TokenExpireDate = DateTime.Now.AddMinutes(15).RoundUpToNearestMinute(); // Używamy tej samej kolumny, to jest OK
                await _context.SaveChangesAsync();

                // 2. Przeniesienie wysyłki do tła (fire and forget)
                // Metoda nie czeka na wysłanie e-maila.
                _ = _emailService.SendPasswordResetEmailAsync(user.Email, resetPasswordToken, user.TokenExpireDate.Value);
                
            }

            // 3. Zawsze zwracaj tę samą, pozytywną odpowiedź
            return Ok(new { success = true, message = "If an account with this email exists, a password reset link has been sent." });
        }

        [HttpGet("validate-token/{token}")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LinkToken == token);

            if (user == null)
                return NotFound(new { valid = false });

            if (user.TokenExpireDate < DateTime.Now)
                return BadRequest("Expired Link");

            return Ok(new { valid = true });
        }

        // 2. Ustawienie nowego hasła (POST)
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Token and new password are required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LinkToken == request.Token);
            if (user == null)
                return BadRequest("Invalid or expired token.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.LinkToken = null;

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
