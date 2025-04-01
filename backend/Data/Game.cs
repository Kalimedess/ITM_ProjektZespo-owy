namespace backend.Data
{
    public class Game
    {
        public int GameId { get; set; }
        public string GameDesc { get; set; } = string.Empty;
        public string GameLongDesc { get; set; } = string.Empty;
        public int BoardId { get; set; }
        public int DecisionDeck { get; set; }
        public int UserId { get; set; }
    }
}