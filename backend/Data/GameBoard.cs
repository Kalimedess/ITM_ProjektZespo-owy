namespace backend.Data
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public int ProcessId{ get; set; }
        public int PozX { get; set; }
        public int PozY { get; set; }
        public int SubboardId { get; set; }
    }
}