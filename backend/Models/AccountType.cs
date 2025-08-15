namespace BankApp.Models
{
    public class AccountType
    {
        public int AccountTypeId { get; set; }

        public string AccountName { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
