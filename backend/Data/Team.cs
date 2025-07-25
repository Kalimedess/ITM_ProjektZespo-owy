using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class Team
    {
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        [MaxLength(7)]
        public string TeamColor { get; set; } = string.Empty;

        [MaxLength(50)]
        public string TeamName { get; set; } = string.Empty;
        public int TeamBud { get; set; }

        [MaxLength(6)]
        public string? TeamToken { get; set; } = string.Empty;

        public int? GameEventId { get; set; } 
        public GameEvent? GameEvent { get; set; }

        public int? TurnsLeft { get; set; }
        public bool IsIndependent { get; set; }

        public virtual ICollection<GameProcess> GameProcesses { get; set; } = new List<GameProcess>();
    }
}