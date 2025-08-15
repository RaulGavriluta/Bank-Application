using BankApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BankApp.Data
{
    public class AccountDataOps
    {
        private readonly BankAppDbContext dbContext;

        public AccountDataOps(BankAppDbContext context)
        {
            dbContext = context;
        }

        public async Task<Account[]> GetAccountsAsync()
        {
            return await dbContext.Accounts.Include(u => u.User).Include(u => u.Type).ToArrayAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(u => u.AccountId == id);
        }

        public async Task AddAccountAsync(Account account)
        {
            await dbContext.AddAsync(account);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            dbContext.Update(account);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (account == null)
                throw new ArgumentException($"Account with id {accountId} does not exist.");

            dbContext.Accounts.Remove(account);
            await dbContext.SaveChangesAsync(); 
        }
    }
}
