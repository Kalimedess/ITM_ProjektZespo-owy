namespace backend.Data
{
    public class DecisionWeight
    {
        public int DecisionWeightId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; } = null!;
        public int DeckId { get; set; }
        public Deck Deck { get; set; } = null!;
        public int WeightX { get; set; }
        public int WeightY { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;
        public double BoosterX { get; set; }
        public double BoosterY { get; set; }
    }
}