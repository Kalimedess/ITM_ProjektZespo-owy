namespace backend.Data
{
    public class Decision
    {
        public int DecisionId { get; set; }
        public int CardId { get; set; }
        public string DecisionShortDesc { get; set; } = string.Empty;
        public string DecisionLongDesc { get; set; } = string.Empty;
        public int DecisionBaseCost { get; set; }
        public int DecisionDeck { get; set;}
    }
}