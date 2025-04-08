using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Board
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string LabelsUp { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string LabelsRight { get; set; } = string.Empty;
        [MaxLength(50)]
        public string DescriptionDown { get; set; } = string.Empty;
        [MaxLength(50)]
        public string DescriptionLeft { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Cols { get; set; }
        [MaxLength(7)]
        public string BorderColor { get; set; } = string.Empty;
        [MaxLength(7)]
        public string CellColor { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string BorderColors { get; set; } = string.Empty;
    }
}
