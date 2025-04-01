namespace backend.Data
{
    public class Item
    {
        public int ItemsId { get; set; }
        public int CardId { get; set; }
        public string HardwareShortDesc { get; set; } = string.Empty;
        public string HardwareLongDesc { get; set; } = string.Empty;
        public int ItemsBaseCost { get; set; }
    }
}