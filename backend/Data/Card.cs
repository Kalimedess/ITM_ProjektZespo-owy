using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Card
    {
        public int CardId { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        [MaxLength(100)]
        public string CardName { get; set; } = string.Empty;
        [Column(TypeName ="ENUM('Decision', 'Item')")]
        public CardType CardType { get; set; }

        public ICollection<DecisionEnabler> DecisionEnablers { get; set; }
        public ICollection<DecisionEnabler> DecisionEnablerOfThis  { get; set; }
        
    }
    public enum CardType { 
        Decision,
         Item
    }
}