namespace backend.Data
{
    public class GameLog
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int  DeckId { get; set; }
        public Deck Deck { get; set; }
        public DateTime Data { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        public int Cost { get; set; }
        public bool Status { get; set; }
    }
}