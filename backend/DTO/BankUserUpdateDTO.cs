using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class BankUserUpdateDTO
    {
        [Required]
        public int BankUserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

    }
}
