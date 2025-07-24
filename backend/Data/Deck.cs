using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class Deck
    {
        public int DeckId { get; set; }
        [MaxLength(50)]
        public string DeckName { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User? User { get; set; }

        public virtual ICollection<Decision> Decisions { get; set; } = new HashSet<Decision>();
        public virtual ICollection<Item> Items { get; set; } = new HashSet<Item>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        public virtual ICollection<Process> Processes { get; set; } = new HashSet<Process>();

    }
}