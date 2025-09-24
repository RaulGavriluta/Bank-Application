using Microsoft.EntityFrameworkCore;
using BankApp.Models;
namespace BankApp.Data
{
    public class BankAppDbContext : DbContext
    {
        public BankAppDbContext(DbContextOptions<BankAppDbContext> options) : base(options)
        {
        }

        public DbSet<BankUser> BankUsers { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<AccountType> AccountTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany()
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AccountType>()
                .HasMany(a => a.Accounts)
                .WithOne(a => a.Type)
                .HasForeignKey(a => a.AccountTypeId);


            // Account Types
            modelBuilder.Entity<AccountType>().HasData(
                new AccountType { AccountTypeId = 1, AccountName = "Deposit" },
                new AccountType { AccountTypeId = 2, AccountName = "Savings" },
                new AccountType { AccountTypeId = 3, AccountName = "Investment" }
            );


        }

    }
}
