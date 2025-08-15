using BankApp.DTO;
using BankApp.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<BankUserDTO[]> GetBankUsersAsync()
        {
            var users = await dbContext.BankUsers
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Type)
            .ToArrayAsync();

            var dtos = users.Select(u => new BankUserDTO
            {
                BankUserId = u.BankUserId,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Password = u.Password,
                Accounts = u.Accounts.Select(a => new AccountDTO
                {
                    AccountId = a.AccountId,
                    IBAN = a.IBAN,
                    Currency = a.Currency,
                    AccountTypeId = a.AccountTypeId,
                    AccountTypeName = a.Type?.AccountName ?? "",
                    BankUserId = a.UserId,
                    BankUserName = a.User?.Name
                }).ToList()
            }).ToArray();

            return dtos;
        }

        public async Task<BankUser?> GetBankUserByIdAsync(int id)
        {
            return await dbContext.BankUsers
                .Include(u => u.Accounts)
                    .ThenInclude(a => a.Type)   
                .FirstOrDefaultAsync(u => u.BankUserId == id);
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
