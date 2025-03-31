namespace backend.Data
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LabelsUp { get; set; } = string.Empty;
        public string LabelsRight { get; set; } = string.Empty;
        public string DescriptionDown { get; set; } = string.Empty;
        public string DescriptionLeft { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Cols { get; set; }
        public string BorderColor { get; set; } = string.Empty;
        public string CellColor { get; set; } = string.Empty;
        public string BorderColors { get; set; } = string.Empty;
    }
}
