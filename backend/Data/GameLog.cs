namespace backend.Data
{
    public class GameLog
    {
        public int GameLogId { get; set; }
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public int DecisionId{ get; set; }
        public int ItemId { get; set; }
        public DateTime Data { get; set; }
        public int feedbackId { get; set; }
        public int Cost { get; set; }
        public int StatusId { get; set; }
    }
}