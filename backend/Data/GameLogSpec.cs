namespace backend.Data
{
    public class GameLogSpec
    {
        public int GameLogSpecId { get; set; }
        public int GameLogId { get; set; }
        public int ProcessId { get; set; }
        public int TeamId { get; set; }
        public int MoveX { get; set; }
        public int MoveY { get; set; }
        public int SubboardId { get; set; }
        public int BoostX { get; set; }
        public int BoostY { get; set; }
    }
}
