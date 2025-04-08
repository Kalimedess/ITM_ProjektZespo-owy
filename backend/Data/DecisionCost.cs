namespace backend.Data
{
    public class DecisionCost
    {
        public int DecisionCostId { get; set; }
        public int DecisionId { get; set; }
        public Decision Decision { get; set; }
        public int DecisionCostWeight { get; set; }
    }
}