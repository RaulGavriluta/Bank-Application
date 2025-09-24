using BankApp.Data;
using BankApp.DTO;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionDataOps _transactionDataOps;

        public TransactionController(ITransactionDataOps transactionDataOps)
        {
            _transactionDataOps = transactionDataOps;
        }

        // GET /api/transaction/{transactionId}
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var transaction = await _transactionDataOps.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
                return NotFound();

            var dto = new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                FromAccountIBAN = transaction.FromAccount?.IBAN ?? "",
                ToAccountIBAN = transaction.ToAccount?.IBAN ?? "",
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                CreatedAt = transaction.CreatedAt
            };

            return Ok(dto);
        }

        // GET /api/transaction/account/{accountId}
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionByAccountId(int accountId)
        {
            var transactions = await _transactionDataOps.GetTransactionByAccountIdAsync(accountId);

            if (transactions == null || transactions.Length == 0)
                return NotFound();

            var dtos = transactions.Select(t => new TransactionDTO
            {
                TransactionId = t.TransactionId,
                FromAccountIBAN = t.FromAccount?.IBAN ?? "",
                ToAccountIBAN = t.ToAccount?.IBAN ?? "",
                Amount = t.Amount,
                Currency = t.Currency,
                CreatedAt = t.CreatedAt
            }).ToList();

            return Ok(dtos);
        }

        // POST /api/transaction
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionDTO transactionDto)
        {
            if (string.IsNullOrEmpty(transactionDto.FromAccountIBAN) || string.IsNullOrEmpty(transactionDto.ToAccountIBAN))
                return BadRequest("Both FromAccountIBAN and ToAccountIBAN are required.");

            var createdTransaction = await _transactionDataOps.CreateTransactionAsync(transactionDto);

            if (createdTransaction == null)
                return BadRequest("Could not create transaction. Check IBANs.");

            var dto = new TransactionDTO
            {
                TransactionId = createdTransaction.TransactionId,
                FromAccountIBAN = createdTransaction.FromAccount?.IBAN ?? "",
                ToAccountIBAN = createdTransaction.ToAccount?.IBAN ?? "",
                Amount = createdTransaction.Amount,
                Currency = createdTransaction.Currency,
                CreatedAt = createdTransaction.CreatedAt
            };

            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = dto.TransactionId }, dto);
        }
    }
}
