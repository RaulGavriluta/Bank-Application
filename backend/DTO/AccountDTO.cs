using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string IBAN { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        public int AccountTypeId { get; set; }

        public string AccountTypeName{ get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public int BankUserId { get; set; }

        public string BankUserName { get; set; }
    }

}
