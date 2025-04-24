namespace backend.Data
{
    public class DecisionWeight
    {
        public int DecisionWeightId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int WeightX { get; set; }
        public int WeightY { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int BoosterX { get; set; }
        public int BoosterY { get; set; }

    }
}