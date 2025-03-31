namespace backend.Data
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int DecisionId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
    }
}