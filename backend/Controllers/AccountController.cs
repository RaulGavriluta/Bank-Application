using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.DTO;
using BankApp.Models;
using BankApp.Interfaces;
using BankApp.Services;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDataOps _accountDataOps;

        public AccountController(IAccountDataOps accountDataOps)
        {
            _accountDataOps = accountDataOps;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountDataOps.GetAccountsAsync();
            var accountDTOs = accounts.Select(a => new AccountDTO
            {
                AccountId = a.AccountId,
                IBAN = a.IBAN,
                Currency = a.Currency,
                AccountTypeId = a.AccountTypeId,
                AccountTypeName = a.Type?.AccountName ?? "",
                BankUserId = a.UserId,
                BankUserName = a.User?.Name ?? "",
                Balance = a.Balance,
                IsActive = a.IsActive
            }).ToList();
            return Ok(accountDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountDataOps.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            var dto = new AccountDTO
            {
                AccountId = account.AccountId,
                IBAN = account.IBAN,
                Currency = account.Currency,
                AccountTypeId = account.AccountTypeId,
                AccountTypeName = account.Type?.AccountName,
                BankUserId = account.UserId,
                BankUserName = account.User?.Name,
                Balance = account.Balance,
                IsActive = account.IsActive
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountDTO dto)
        {
            var account = new Account
            {
                IBAN = GenerateIBAN.Generate(),
                Currency = dto.Currency,
                AccountTypeId = dto.AccountTypeId,
                UserId = dto.BankUserId,
                IsActive = true,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };
            await _accountDataOps.AddAccountAsync(account);
            var resultDto = new AccountDTO
            {
                AccountId = account.AccountId,
                IBAN = account.IBAN,
                Currency = account.Currency,
                AccountTypeId = account.AccountTypeId,
                AccountTypeName = account.Type?.AccountName ?? "",
                BankUserId = account.UserId,
                BankUserName = account.User?.Name ?? "",
                Balance = account.Balance,
                IsActive = account.IsActive
            };
            return Ok(resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountUpdateDTO dto)
        {
            if (id != dto.AccountId)
            {
                return BadRequest("Account ID mismatch.");
            }
            var account = await _accountDataOps.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            account.Currency = dto.Currency;
            account.UserId = dto.BankUserId;
            account.IsActive = dto.IsActive;
            account.AccountTypeId = dto.AccountTypeId;
            await _accountDataOps.UpdateAccountAsync(account);

            var resultDto = new AccountUpdateDTO
            {
                AccountId = account.AccountId,
                Currency = account.Currency,
                AccountTypeId = account.AccountTypeId,
                AccountTypeName = account.Type?.AccountName ?? "",
                BankUserId = account.UserId,
                IsActive = account.IsActive
            };
            return Ok(resultDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountDataOps.DeleteAccountAsync(id);
                return Ok(new { message = "Account deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
