namespace backend.Data
{
    public class Item
    {
        public int Items_id { get; set; }
        public int Card_id { get; set; }
        public string HardwareShortDesc { get; set; } = string.Empty;
        public string HardwareLongDesc { get; set; } = string.Empty;
        public int ItemsBaseCost { get; set; }
    }
}