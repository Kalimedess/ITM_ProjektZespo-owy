namespace backend.Data
{
    public class Game
    {
        public int Game_id { get; set; }
        public string Game_desc { get; set; } = string.Empty;
        public string Game_long_desc { get; set; } = string.Empty;
        public int Board_id { get; set; }
        public string Game_Token { get; set; } = string.Empty;
        public int DecisionDeck { get; set; }
    }
}