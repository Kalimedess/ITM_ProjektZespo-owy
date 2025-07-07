using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class GameProcess
    {
        public int GameProcessId { get; set; }
        [MaxLength(100)]
        public string ProcessDesc { get; set; } = string.Empty;

        public int? GameId { get; set; }
        public Game? Game { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}