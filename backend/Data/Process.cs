using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class Process
    {
        public int ProcessId { get; set; }
        [MaxLength(25)]
        public string ProcessDesc { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ProcessLongDesc { get; set; } = string.Empty;
        [MaxLength(7)]
        public string ProcessColor { get; set; } = string.Empty;
        public int DeckId { get; set; }
        public Deck Deck { get; set; } = null!;
    }
}