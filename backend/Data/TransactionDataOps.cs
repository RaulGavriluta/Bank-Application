using Microsoft.EntityFrameworkCore;
using BankApp.Models;
namespace BankApp.Data
{
    public class TransactionDataOps
    {
        private readonly BankAppDbContext dbContext;

        public TransactionDataOps(BankAppDbContext context)
        {
            dbContext = context;
        }

        public async Task<Transaction[]> GetTransactionsAsync()
        {
            return await dbContext.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToArrayAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await dbContext.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(u => u.TransactionId == id);
        }

        public async Task<Transaction[]> GetTransactionByAccountIdAsync(int accountId)
        {
            return await dbContext.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToArrayAsync();
        }

        public async Task<Transaction[]> GetTransactionByUserIdAsync(int userId)
        {
            return await dbContext.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Where(t => t.FromAccount.UserId == userId || t.ToAccount.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<Transaction[]> GetTransactionsByTypeAsync(TransactionType type)
        {
            return await dbContext.Transactions
                .Where(t => t.Type == type)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToArrayAsync();
        }
    }
}
