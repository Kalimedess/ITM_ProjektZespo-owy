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
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, EmailService emailService, JwtService jwtService, IConfiguration configuration)  
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _jwtService = jwtService;
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
                user.ConfirmationToken = Guid.NewGuid().ToString();
                await _context.SaveChangesAsync();

                await SendConfirmationEmail(user.Email);

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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Email not found.");
            }


            var ResetPasswordToken = Guid.NewGuid().ToString();
            user.ConfirmationToken = ResetPasswordToken;
            await _context.SaveChangesAsync();
            var frontendBaseUrl = _configuration.GetValue<string>("CorsSettings:AllowedOrigins:0");
            var ResetPasswordLink = $"{frontendBaseUrl}/resetPassword/{ResetPasswordToken}";

            if (string.IsNullOrEmpty(ResetPasswordLink))
            {
                ResetPasswordLink = $"{frontendBaseUrl}/resetPassword/{ResetPasswordToken}";
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, "reset hasla", $"Reset Password: {ResetPasswordLink}");
                return Ok(new { success = true, message = "Reset Password test" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending reset email: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "failed to send reset email.");
            }
            
        }  
        private async Task<bool> SendConfirmationEmail(string email)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    var frontendBaseUrl = _configuration.GetValue<string>("CorsSettings:AllowedOrigins:0");
    var confirmationLink = $"{frontendBaseUrl}/confirm/{user.ConfirmationToken}";
    
    if (string.IsNullOrEmpty(confirmationLink))
            {
                confirmationLink = $"{frontendBaseUrl}/confirm/{user.ConfirmationToken}";
            }

    try
    {
        await _emailService.SendEmailAsync(user.Email, "Potwierdzenie rejestracji", $"Kliknij w link aby aktywować konto: {confirmationLink}");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending confirmation email: {ex.Message}");
        return false;
    }
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
                ConfirmationToken = confirmationToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // --- Kopiowanie szablonowego Decku wraz z zawartością ---
            int templateDeckId = 1;
            var templateDeck = await _context.Decks
                .Include(d => d.Decisions)
                .Include(d => d.Items)
                .Include(d => d.Feedbacks)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DeckId == templateDeckId && d.UserId == null);

            if (templateDeck != null)
            {
                // Stwórz nowy Deck dla użytkownika
                var newDeckForUser = new Deck
                {
                    DeckName = $"Podstawowy",
                    UserId = user.UserId
                };
                _context.Decks.Add(newDeckForUser);
                await _context.SaveChangesAsync();
                Console.WriteLine($"New deck {newDeckForUser.DeckId} created for user {user.UserId}.");

                // Kopiuj Decisions
                int decisionsPrepared = 0;
                if (templateDeck.Decisions != null && templateDeck.Decisions.Any())
                {
                    foreach (var decisionTemplate in templateDeck.Decisions)
                    {
                        _context.Decisions.Add(new Decision
                        {
                            DeckId = newDeckForUser.DeckId,
                            CardId = decisionTemplate.CardId,
                            DecisionShortDesc = decisionTemplate.DecisionShortDesc,
                            DecisionLongDesc = decisionTemplate.DecisionLongDesc,
                            DecisionBaseCost = decisionTemplate.DecisionBaseCost,
                            DecisionCostWeight = decisionTemplate.DecisionCostWeight,
                        });
                        decisionsPrepared++;
                    }
                }
                Console.WriteLine($"{decisionsPrepared} Decisions prepared for Deck {newDeckForUser.DeckId}.");

                // Kopiuj Items
                if (templateDeck.Items != null)
                {
                    foreach (var itemTemplate in templateDeck.Items)
                    {
                        _context.Items.Add(new Item
                        {
                            DeckId = newDeckForUser.DeckId,
                            CardId = itemTemplate.CardId,
                            HardwareShortDesc = itemTemplate.HardwareShortDesc,
                            HardwareLongDesc = itemTemplate.HardwareLongDesc,
                            ItemsBaseCost = itemTemplate.ItemsBaseCost,
                            ItemsCostWeight = itemTemplate.ItemsCostWeight
                        });
                    }
                }

                // Kopiuj Feedbacks
                if (templateDeck.Feedbacks != null)
                {
                    foreach (var feedbackTemplate in templateDeck.Feedbacks)
                    {
                        _context.Feedbacks.Add(new Feedback
                        {
                            DeckId = newDeckForUser.DeckId,
                            CardId = feedbackTemplate.CardId,
                            Status = feedbackTemplate.Status,
                            LongDescription = feedbackTemplate.LongDescription,
                            FeedbackPDF = feedbackTemplate.FeedbackPDF
                        });
                    }
                }
            }
            else
            {
                Console.WriteLine($"WARNING: Template Deck ID {templateDeckId} (UserId NULL) NOT FOUND. Cannot copy deck contents for user {user.Email}.");
            }


            // --- Kopiowanie szablonowej Board ---
            int[] templateBoardIds = { 1, 2, 3 };

            foreach (var templateBoardId in templateBoardIds)
            {
                var templateBoard = await _context.Boards
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BoardId == templateBoardId && b.UserId == null);

                if (templateBoard != null)
                {
                    _context.Boards.Add(new Board
                    {
                        UserId = user.UserId,
                        Name = $"{templateBoard.Name}",
                        LabelsUp = templateBoard.LabelsUp,
                        LabelsRight = templateBoard.LabelsRight,
                        DescriptionDown = templateBoard.DescriptionDown,
                        DescriptionLeft = templateBoard.DescriptionLeft,
                        Rows = templateBoard.Rows,
                        Cols = templateBoard.Cols,
                        BorderColor = templateBoard.BorderColor,
                        CellColor = templateBoard.CellColor,
                        BorderColors = templateBoard.BorderColors
                    });
                }
            }

            await _context.SaveChangesAsync();

            if (await SendConfirmationEmail(user.Email))
            {
                return Ok(new { success = true, message = "Registration successful. Please check your email." });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Registration succeeded but failed to send confirmation email.");
            }

        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid or expired confirmation token.");
            }

            if (user.EmailConfirmed)
            {
                return Ok(new { success = true, message = "Email already confirmed." });
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

            return Ok(new
            {
                id = user.UserId,
                name = user.Name,
                email = user.Email
            });
        }

    }
}