using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class GameProcess
    {
        public int GameProcessId { get; set; }
        [MaxLength(10)]
        public string ProcessDesc { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ProcessLongDesc { get; set; } = string.Empty;

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}