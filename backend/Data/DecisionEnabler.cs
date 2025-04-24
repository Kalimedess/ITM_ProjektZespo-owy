namespace backend.Data
{
    public class DecisionEnabler
    {
        public int DecisionEnablerId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int EnablerId { get; set; }
        public Card CardEnabler { get; set; }
    }
}