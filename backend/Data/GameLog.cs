namespace backend.Data
{
    public class GameLog
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int DecisionId{ get; set; }
        public Decision Decision { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime Data { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        public int Cost { get; set; }
        public int StatusId { get; set; }
    }
}