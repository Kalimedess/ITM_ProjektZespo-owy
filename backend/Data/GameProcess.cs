using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data
{
    public class GameProcess
    {
        public int GameProcessId { get; set; }
        [MaxLength(100)]
        public string ProcessDesc { get; set; } = string.Empty;
        [Column(TypeName = "TEXT")]
        public string ProcessLongDesc { get; set; } = string.Empty;
    }
}


