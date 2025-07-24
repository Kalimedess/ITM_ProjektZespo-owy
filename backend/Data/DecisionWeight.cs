namespace backend.Data
{
    public class DecisionWeight
    {
        public int DecisionWeightId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; } = null!;
        public int ProcessId { get; set; }
        public Process Process { get; set; } = null!;
        public int DeckId { get; set; }
        public Deck Deck { get; set; } = null!;
        public int WeightX { get; set; }
        public int WeightY { get; set; }
        public double BoosterX { get; set; }
        public double BoosterY { get; set; }
    }
}