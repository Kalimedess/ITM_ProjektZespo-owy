using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class GameEvent
    {
        public int GameEventId { get; set; }

        [MaxLength(100)]
        public string EventShortDesc { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string EventLongDesc { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User User { get; set; } = null!;
        public int TurnTime { get; set; }
        public double? DecisionCostWeight { get; set; }
        public double? ItemsCostWeight { get; set; }
        public double? BoosterX { get; set; }
        public double? BoosterY { get; set; }
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
        public virtual ICollection<GameLog> GameLogs { get; set; } = new List<GameLog>();
    }
}