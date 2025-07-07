using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Card
    {
        public int CardId { get; set; }
        [Column(TypeName ="ENUM('Decision', 'Item')")]
        public CardType CardType { get; set; }
        public ICollection<DecisionEnabler> DecisionEnablers { get; set; } = new List<DecisionEnabler>();
        public ICollection<DecisionEnabler> DecisionEnablerOfThis  { get; set; } = new List<DecisionEnabler>();
    }
    
    public enum CardType { 
        Decision,
        Item
    }
}