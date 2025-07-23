namespace backend.Data
{
    public class GameLogSpec
    {
        public int GameLogSpecId { get; set; }
        public int GameLogId { get; set; }
        public int? GameProcessId { get; set; }
        public GameProcess? GameProcess { get; set; }
        public int MoveX { get; set; }
        public int MoveY { get; set; }
    }
}