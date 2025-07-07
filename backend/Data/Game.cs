using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace backend.Data
{
    public class Game
    {
        public int GameId { get; set; }
        [MaxLength(50)]
        public string GameDesc { get; set; } = string.Empty;

        public int TeamBoardId { get; set; }
        public Board TeamBoard { get; set; }

        public int RivalBoardId { get; set; }
        public Board RivalBoard { get; set; }

        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        [Column(TypeName = "ENUM('During', 'Paused', 'End')")]
        public GameStatus? GameStatus { get; set; }

        public bool IsOnline { get; set; }
        
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
        public virtual ICollection<GameBoard> GameBoards { get; set; } = new List<GameBoard>();
        public virtual ICollection<GameLog> GameLogs { get; set; } = new List<GameLog>();
    }

    public enum GameStatus
    {
        During,
        Paused,
        End
    }
}