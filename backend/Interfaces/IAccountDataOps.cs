using BankApp.Models;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface IAccountDataOps
    {
        // Returnează toate conturile
        Task<Account[]> GetAccountsAsync();

        // Returnează un cont după ID
        Task<Account?> GetAccountByIdAsync(int id);

        // Adaugă un cont nou
        Task AddAccountAsync(Account account);

        // Actualizează un cont existent
        Task UpdateAccountAsync(Account account);

        // Șterge un cont după ID
        Task DeleteAccountAsync(int accountId);
    }
}
