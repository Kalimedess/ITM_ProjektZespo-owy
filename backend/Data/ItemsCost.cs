namespace backend.Data
{
    public class ItemsCost
    {
        public int ItemsCostId { get; set; }
        public int ItemsId { get; set; }
        public Item Item { get; set; }
        public int ItemsCostWeight { get; set; }
    }
}