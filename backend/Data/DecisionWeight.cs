namespace backend.Data
{
    public class DecisionWeight
    {
        public int DecisionWeightId { get; set; }
        public int DecisionId { get; set; }
        public Decision Decision { get; set; }
        public int WeightX { get; set; }
        public int WeightY { get; set; }
        public int SubboardId { get; set; }
        public int BoosterX { get; set; }
        public int BoosterY { get; set; }

    }
}