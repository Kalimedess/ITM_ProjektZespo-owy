namespace backend.Data
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int? GameProcessId{ get; set; }
        public GameProcess? GameProcess { get; set; }
        public double PozX { get; set; }
        public double PozY { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;
    }
}