using System;
using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }

        [Required]
        public string FromAccountIBAN { get; set; }

        [Required]
        public string ToAccountIBAN { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
