namespace backend.Data
{
    public class GameBoard
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int ProcessId{ get; set; }
        public GameProcess GameProcess { get; set; }
        public int PozX { get; set; }
        public int PozY { get; set; }
        public int SubboardId { get; set; }
    }
}