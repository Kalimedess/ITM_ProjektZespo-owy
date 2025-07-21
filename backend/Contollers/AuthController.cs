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

public class ResetPasswordRequest {
    public string Email { get; set; } = string.Empty;
}

namespace backend.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly IUserInitializationService _initializationService;

        public AuthController(AppDbContext context, IEmailService emailService, JwtService jwtService, IConfiguration configuration, IUserInitializationService initializationService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _jwtService = jwtService;
            _initializationService = initializationService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Username || u.Name == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            if (!user.EmailConfirmed)
            {
                // wygeneruj nowy token
                user.LinkToken = Guid.NewGuid().ToString();
                user.TokenExpireDate = DateTime.Now.AddMinutes(15).RoundUpToNearestMinute();
                await _context.SaveChangesAsync();

                _ = _emailService.SendConfirmationEmailAsync(user.Email, user.LinkToken, user.TokenExpireDate.Value);

                return BadRequest("E-mail nie został potwierdzony. Wysłano ponownie link aktywacyjny.");
            }


            var claims = JwtService.GenerateToken(user);

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

            return Ok(new
            {
                success = true,
                user = new
                {
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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email already exist.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var confirmationToken = Guid.NewGuid().ToString();

            var user = new User
            {
                Name = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                EmailConfirmed = false,
                LinkToken = confirmationToken,
                TokenExpireDate = DateTime.Now.AddMinutes(15).RoundUpToNearestMinute()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 1. Inicjalizacja danych w bazie
            _ = _initializationService.InitializeUserAsync(user.UserId);

            // 2. Wysyłka e-maila
            _ = _emailService.SendConfirmationEmailAsync(user.Email, user.LinkToken, user.TokenExpireDate.Value);

            // --- Zwróć odpowiedź NATYCHMIAST ---
            return Ok(new { success = true, message = "Registration successful. Please check your email for a confirmation link." });

        }


        [HttpGet("confirm/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            Console.WriteLine("Potwierdzanie maila");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LinkToken == token);

            if (user == null)
            {
                return BadRequest("Invalid or expired confirmation token.");
            }
            Console.WriteLine($"Obecny czas {DateTime.Now}");
            if (user.TokenExpireDate < DateTime.Now)
            {
                return BadRequest("Expired Link");
            }

            if (user.EmailConfirmed)
                {
                    return Ok(new { success = true, message = "Email already confirmed." });
                }

            user.EmailConfirmed = true;
            user.LinkToken = null;
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

            return Ok(new
            {
                id = user.UserId,
                name = user.Name,
                email = user.Email
            });
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime RoundUpToNearestMinute(this DateTime dateTime)
        {
            if (dateTime.Second > 0 || dateTime.Millisecond > 0)
            {
                dateTime = dateTime.AddMinutes(1);
            }

            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0,
                dateTime.Kind
            );
        }
    }
}
