using System.Diagnostics;

namespace backend.Data
{
    public class GameLogSpec
    {
        public int GameLogSpecId { get; set; }
        public int GameLogId { get; set; }
        public GameLog GameLog { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int GameProcessId { get; set; }
        public GameProcess GameProcess { get; set; }
        public int MoveX { get; set; }
        public int MoveY { get; set; }
        public int BoostX { get; set; }
        public int BoostY { get; set; }
    }
}
