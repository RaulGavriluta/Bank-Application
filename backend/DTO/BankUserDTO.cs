using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class BankUserDTO
    {
        public int BankUserId { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        public List<AccountDTO> Accounts { get; set; }

    }
}
