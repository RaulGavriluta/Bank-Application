using BankApp.DTO;
using BankApp.Models;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface ITransactionDataOps
    {
        // Returnează o tranzacție după ID
        Task<Transaction?> GetTransactionByIdAsync(int id);

        // Returnează toate tranzacțiile asociate unui cont (from sau to)
        Task<Transaction[]> GetTransactionByAccountIdAsync(int accountId);

        // Creează o tranzacție folosind un DTO
        Task<Transaction?> CreateTransactionAsync(TransactionDTO dto);
    }
}
