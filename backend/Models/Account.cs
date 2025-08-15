using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        public int UserId { get; set; }
        public BankUser User { get; set; }

        [Required]
        [StringLength(34)]
        public string IBAN { get; set; }
        public decimal Balance { get; set; } = 0;

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        public int AccountTypeId { get; set; }

        public AccountType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
