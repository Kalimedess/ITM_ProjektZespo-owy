using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "TEXT")]
        public string LongDescription { get; set; } = string.Empty;
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        [Column(TypeName = "LONGBLOB")]
        public byte[]? FeedbackPDF { get; set; }
    }
}