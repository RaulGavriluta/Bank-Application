using BankApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BankApp.Data
{
    public class BankUserDataOps
    {
        private readonly BankAppDbContext dbContext;

        public BankUserDataOps(BankAppDbContext context)
        {
            dbContext = context;
        }

        public async Task<BankUser[]> GetBankUsersAsync()
        {
            return await dbContext.BankUsers.Include(u => u.Accounts).ToArrayAsync();
        }

        public async Task<BankUser?> GetBankUserByIdAsync(int id)
        {
            return await dbContext.BankUsers.FirstOrDefaultAsync(u => u.BankUserId == id);
        }

        public async Task AddBankUserAsync(BankUser user)
        {
            await dbContext.BankUsers.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateBankUserAsync(BankUser user)
        {
            dbContext.BankUsers.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBankUserAsync(int userId)
        {
            var user = await dbContext.BankUsers.FirstOrDefaultAsync(a => a.BankUserId == userId);
            if (user == null)
                throw new ArgumentException($"Bank user with id {userId} not found.");

            dbContext.BankUsers.Remove(user);
            await dbContext.SaveChangesAsync();
        }    
    }
}
