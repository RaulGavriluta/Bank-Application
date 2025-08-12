using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class BankUser
    {
        public int BankUserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
