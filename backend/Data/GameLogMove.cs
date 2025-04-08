using System.Diagnostics;

namespace backend.Data
{
    public class GameLogMove
    {
        public int GameLogMoveId { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int ProcessId{ get; set; }
        public GameProcess GameProcess { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int MoveX { get; set; }
        public int MoveY { get; set; }
        public int SubBoard { get; set; }
    }
}
