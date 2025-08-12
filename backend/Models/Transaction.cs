using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public enum TransactionType
    {
        Transfer = 1,
        Deposit = 2,
        Withdrawal = 3
    }
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }
        [Required]
        public int FromAccountId { get; set; }
        public Account FromAccount { get; set; }
        [Required]
        public int ToAccountId{ get; set; }
        public Account ToAccount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public TransactionType Type { get; set; } // "Transfer", "Deposit" , "Withdrawal"
        [Required]
        public string Status { get; set; } = "Pending";
    }
}
