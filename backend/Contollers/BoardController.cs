 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers
{
    public class BoardCreateDto
    {
    [Required(ErrorMessage = "Nazwa planszy jest wymagana.")]
    [StringLength(50, ErrorMessage = "Nazwa planszy nie może przekraczać 50 znaków.")]
    public string Name { get; set; } = string.Empty;

    public string? LabelsUp { get; set; }
    public string? LabelsRight { get; set; }

    [StringLength(50, ErrorMessage = "Opis dolny nie może przekraczać 50 znaków.")]
    public string? DescriptionDown { get; set; }

    [StringLength(50, ErrorMessage = "Opis lewy nie może przekraczać 50 znaków.")]
    public string? DescriptionLeft { get; set; }

    [Range(1, 100, ErrorMessage = "Liczba wierszy musi być między 1 a 100.")]
    public int Rows { get; set; }

    [Range(1, 100, ErrorMessage = "Liczba kolumn musi być między 1 a 100.")]
    public int Cols { get; set; } 

    [Required]
    [StringLength(7, ErrorMessage = "Kod koloru obramowania musi mieć 7 znaków (np. #RRGGBB).")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Nieprawidłowy format koloru heksadecymalnego.")]
    public string BorderColor { get; set; } = string.Empty;

    [Required]
    [StringLength(7, ErrorMessage = "Kod koloru komórki musi mieć 7 znaków (np. #RRGGBB).")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Nieprawidłowy format koloru heksadecymalnego.")]
    public string CellColor { get; set; } = string.Empty;

    public string? BorderColors { get; set; }
    }



    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BoardController(AppDbContext context)
        {
            _context = context;
        }
        
        [Authorize]        
        [HttpGet("get")]
        public async Task<IActionResult> GetBoardsForUser()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized();

        var boards = await _context.Boards
        .Where(b => b.UserId == userId)
        .ToListAsync();

            return Ok(boards);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardCreateDto boardDto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {

                return Unauthorized("User ID claim is missing or invalid.");
            }

            var newBoard = new Board
            {
                UserId = userId,
                Name = boardDto.Name,
                LabelsUp = boardDto.LabelsUp,
                LabelsRight = boardDto.LabelsRight,
                DescriptionDown = boardDto.DescriptionDown,
                DescriptionLeft = boardDto.DescriptionLeft,
                Rows = boardDto.Rows,
                Cols = boardDto.Cols,
                CellColor = boardDto.CellColor,
                BorderColor = boardDto.BorderColor,
                BorderColors = boardDto.BorderColors
            };

            _context.Boards.Add(newBoard);
            await _context.SaveChangesAsync();

            return Ok(newBoard);
        }

        [Authorize]
        [HttpPut("edit/{id:int}")]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] BoardCreateDto boardDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Board ID.");
            }

            var boardToUpdate = await _context.Boards.FindAsync(id);

            if (boardToUpdate == null)
            {
                return NotFound($"Board with ID {id} not found.");
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            if (boardToUpdate.UserId == null || boardToUpdate.UserId != userId)
            {
                return Forbid("You do not have permission to edit this board.");
            }

            boardToUpdate.Name = boardDto.Name;
            boardToUpdate.LabelsUp = boardDto.LabelsUp ?? string.Empty;
            boardToUpdate.LabelsRight = boardDto.LabelsRight ?? string.Empty;
            boardToUpdate.DescriptionDown = boardDto.DescriptionDown ?? string.Empty;
            boardToUpdate.DescriptionLeft = boardDto.DescriptionLeft ?? string.Empty;
            boardToUpdate.Rows = boardDto.Rows;
            boardToUpdate.Cols = boardDto.Cols;
            boardToUpdate.CellColor = boardDto.CellColor;
            boardToUpdate.BorderColor = boardDto.BorderColor;
            boardToUpdate.BorderColors = boardDto.BorderColors ?? string.Empty;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The board was modified by another user. Please refresh and try again.");
            }

            return Ok(boardToUpdate);
        }
        
        [Authorize]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("User ID claim is missing or invalid.");
            }

            var boardToDelete = await _context.Boards.FindAsync(id);
            if (boardToDelete == null)
            {
                return NotFound($"Board with ID {id} not found.");
            }

            if (boardToDelete.UserId == null || boardToDelete.UserId != userId)
            {
                return Forbid("You do not have permission to delete this board.");
            }

            _context.Boards.Remove(boardToDelete);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}