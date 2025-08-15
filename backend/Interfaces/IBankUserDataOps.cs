using BankApp.Data;
using BankApp.DTO;
using BankApp.Models;
namespace BankApp.Interfaces
{
    public interface IBankUserDataOps
    {
        Task DeleteBankUserAsync(int id);

        Task<BankUserDTO[]> GetBankUsersAsync(); 
        
        Task<BankUser?> GetBankUserByIdAsync(int id);
        Task AddBankUserAsync(BankUser user);
        Task UpdateBankUserAsync(BankUser user);
    }
}
