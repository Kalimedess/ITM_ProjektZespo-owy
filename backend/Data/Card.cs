using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Card
    {
        public int CardId { get; set; }
        [Column(TypeName ="ENUM('Decision', 'Item')")]
        public CardType CardType { get; set; }

        public double BaseCost { get; set; }
        public int CostWeight { get; set; }
        public ICollection<DecisionEnabler> DecisionEnablers { get; set; }
        public ICollection<DecisionEnabler> DecisionEnablerOfThis  { get; set; }
        
    }
    public enum CardType { 
        Decision,
         Item
    }
}