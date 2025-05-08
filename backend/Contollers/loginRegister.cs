using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


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
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, EmailService emailService, JwtService jwtService) {
            _context = context;
            _emailService = emailService;
            _jwtService = jwtService;
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new {success = true, 
                            user = new {
                                id = user.UserId,
                                name = user.Name, 
                                email = user.Email
                                }
                        });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { success = true, message = "Wylogowano pomyślnie" });
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

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token = confirmationToken }, Request.Scheme);

            if (string.IsNullOrEmpty(confirmationLink))
            {
                 confirmationLink = $"{(Request.IsHttps ? "https" : "http")}://{Request.Host}/api/auth/confirm?token={confirmationToken}";
            }


            try
            {
                await _emailService.SendEmailAsync(user.Email, "Potwierdzenie rejestracji", $"Kliknij w link aby aktywować konto: {confirmationLink}");
                return Ok(new { success = true, message = "Registration successful. Please check your email." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending confirmation email: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Registration succeeded but failed to send confirmation email.");
            }
        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token)) {
                return BadRequest("Invalid token.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid or expired confirmation token.");
            }

            if (user.EmailConfirmed) {
                return Ok(new { success = true, message = "Email already confirmed."});
            }

            user.EmailConfirmed = true;
            user.ConfirmationToken = null;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Email confirmed successfully. You can now log in." });
        }

        
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new { message = "User ID not found in claims." });
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Invalid user ID format in claims." });
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized(new { message = "User not found or session expired." });
            }

            return Ok(new {
                id = user.UserId,
                name = user.Name,
                email = user.Email
            });
        }

    }
}