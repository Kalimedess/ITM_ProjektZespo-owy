using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int DecisionId { get; set; }
        public Decision Decision { get; set; }
        [MaxLength(2)]
        public string Status { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string LongDescription { get; set; } = string.Empty;
    }
}