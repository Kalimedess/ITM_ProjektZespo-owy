using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class GameProcess
    {
        public int GameProcessId { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; } = null!;

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}