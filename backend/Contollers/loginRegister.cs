using backend.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Services;

public class LoginRequest {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest {
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EmailConfirmed{ get; set; } = false;
    public string ConfirmationToken{ get; set; } = string.Empty;
}


namespace backend.Controllers {

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, EmailService emailService) {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password)) {
                return BadRequest("Username and password are required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Username || u.Name == request.Username);

            if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) {
                return Unauthorized("Invalid credentials");
            }

            if (!user.EmailConfirmed) {
                return BadRequest("E-mail nie został potwierdzony. Sprawdź skrzynkę pocztową.");
            }

            return Ok(new {success = true});
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
            if(await _context.Users.AnyAsync(u => u.Email == request.Email)) {
                return BadRequest("Email already exist.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var confirmationToken = Guid.NewGuid().ToString();

            var user = new User {
                Name = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                EmailConfirmed = false,
                ConfirmationToken = confirmationToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var confirmationLink = $"http://localhost:5023/api/auth/confirm?token={confirmationToken}";

            await _emailService.SendEmailAsync(user.Email, "Potwierdzenie rejestracji", $"Kliknij w link aby aktywować konto: {confirmationLink}");

            return Ok(new {success = true});
        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if(user == null) {
                return BadRequest("Nieprawidłowy token");
            }

            user.EmailConfirmed = true;
            user.ConfirmationToken = null;
            await _context.SaveChangesAsync();

            return Ok(new {success = true});
        }

    }
}