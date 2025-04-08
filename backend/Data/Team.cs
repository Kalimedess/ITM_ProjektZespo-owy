using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class Team
    {
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        [MaxLength(50)]
        public string TeamName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string TeamLeader { get; set; } = string.Empty;
        public int TeamBud { get; set; }
        [MaxLength(6)]
        public string TeamToken { get; set; } = string.Empty;
    }
}