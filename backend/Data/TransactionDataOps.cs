using BankApp.DTO;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class TransactionDataOps : ITransactionDataOps
    {
        private readonly BankAppDbContext dbContext;

        public TransactionDataOps(BankAppDbContext context)
        {
            dbContext = context;
        }

        // Returnează o tranzacție după ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await dbContext.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        // Returnează toate tranzacțiile asociate unui cont
        public async Task<Transaction[]> GetTransactionByAccountIdAsync(int accountId)
        {
            return await dbContext.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToArrayAsync();
        }

        // Creează o tranzacție folosind doar IBAN-uri
        public async Task<Transaction?> CreateTransactionAsync(TransactionDTO dto)
        {
            // Căutăm conturile după IBAN
            var fromAccount = await dbContext.Accounts
                .FirstOrDefaultAsync(a => a.IBAN == dto.FromAccountIBAN);

            var toAccount = await dbContext.Accounts
                .FirstOrDefaultAsync(a => a.IBAN == dto.ToAccountIBAN);

            if (fromAccount == null || toAccount == null)
            {
                throw new Exception("Please provide both IBAN's");
            }else if(fromAccount == toAccount)
            {
                throw new InvalidOperationException("The account cannot be the same");
            }


            if (fromAccount.Balance < dto.Amount)
            {
                throw new InvalidOperationException("Insuficient funds");
            }

            using var dbTransaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                fromAccount.Balance -= dto.Amount;

                toAccount.Balance += dto.Amount;

                var transaction = new Transaction
                {
                    FromAccountId = fromAccount.AccountId,
                    ToAccountId = toAccount.AccountId,
                    Amount = dto.Amount,
                    Currency = dto.Currency,
                    CreatedAt = DateTime.UtcNow,
                    FromAccount = fromAccount,
                    ToAccount = toAccount
                };

                await dbContext.Transactions.AddAsync(transaction);
                await dbTransaction.CommitAsync();
                await dbContext.SaveChangesAsync();

                return transaction;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
