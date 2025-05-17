using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Decision
    {
        public int DecisionId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int DeckId { get; set;}
        public Deck Deck { get; set; }
        [MaxLength(100)]
        public string DecisionShortDesc { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string DecisionLongDesc { get; set; } = string.Empty;
        
    }
}