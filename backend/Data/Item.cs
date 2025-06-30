using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Item
    {
        public int ItemsId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        [MaxLength(100)]
        public string HardwareShortDesc { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string HardwareLongDesc { get; set; } = string.Empty;
        
        public double ItemsBaseCost { get; set; }
        public int ItemsCostWeight { get; set; }
        
    }
}