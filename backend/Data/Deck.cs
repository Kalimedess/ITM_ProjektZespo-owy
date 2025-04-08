using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class Deck
    {
        public int DeckId { get; set; }
        [MaxLength(50)]
        public string DeckName { get; set; } = string.Empty;

    }
}