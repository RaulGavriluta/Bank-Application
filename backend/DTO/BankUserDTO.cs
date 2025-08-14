using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class BankUserDTO
    {

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

    }
}
