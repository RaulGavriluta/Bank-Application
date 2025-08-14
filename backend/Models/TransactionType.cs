namespace BankApp.Models
{
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        
    }
}
