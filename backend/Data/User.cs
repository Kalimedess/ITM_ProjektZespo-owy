using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class User
    {
        public int UserId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        [MaxLength(50)]
        public string? LinkToken { get; set; }
        public DateTime? TokenExpireDate { get; set; }
        public int LicensesOwned { get; set; }
        public int LicensesUsed { get; set; }

    }
}
