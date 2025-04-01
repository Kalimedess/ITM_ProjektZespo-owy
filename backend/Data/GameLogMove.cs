namespace backend.Data
{
    public class GameLogMove
    {
        public int GameLogMoveId { get; set; }
        public int BoardId { get; set; }
        public int ProcessId{ get; set; }
        public int TeamId { get; set; }
        public int MoveX { get; set; }
        public int MoveY { get; set; }
        public int SubBoard { get; set; }
    }
}
