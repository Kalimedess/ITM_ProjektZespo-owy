using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class Game
    {
        public int GameId { get; set; }
        [MaxLength(50)]
        public string GameDesc { get; set; } = string.Empty;
        [MaxLength(255)]
        public string GameLongDesc { get; set; } = string.Empty;
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}