using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class LoginRequest {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


namespace backend.Controllers {

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context) {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password)) {
                return BadRequest("Username and password are required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Username);

            if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new {success = true});
        }
    }
}