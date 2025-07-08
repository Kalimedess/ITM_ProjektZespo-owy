namespace backend.Data
{
    public class DecisionEnabler
    {
        public int DecisionEnablerId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; } = null!;
        public int? EnablerId { get; set; }
        public Card? CardEnabler { get; set; }

        public int? GameId { get; set; }
        public Game? Game { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}