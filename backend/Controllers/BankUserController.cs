using BankApp.Data;
using BankApp.DTO;
using Microsoft.AspNetCore.Mvc;
using BankApp.Models;
namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankUserController : ControllerBase
    {
        private readonly BankUserDataOps _bankUserDataOps;

        public BankUserController(BankUserDataOps bankUserDataOps)
        {
            _bankUserDataOps = bankUserDataOps;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBankUsers()
        {
            var users = await _bankUserDataOps.GetBankUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankUserById(int id)
        {
            var user = await _bankUserDataOps.GetBankUserByIdAsync(id);

            if (user == null)
                return NotFound();

            var dto = new BankUserDTO
            {
                BankUserId = user.BankUserId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                Accounts = user.Accounts.Select(a => new AccountDTO
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
                   
                }).ToList()
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBankUser(BankUserDTO dto)
        {
            var user = new BankUser
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password
            };

            await _bankUserDataOps.AddBankUserAsync(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBankUser(int id, BankUserUpdateDTO dto)
        {
            if (id != dto.BankUserId)
            {
                return BadRequest("User ID mismatch.");
            }
            var user = await _bankUserDataOps.GetBankUserByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            await _bankUserDataOps.UpdateBankUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankUser(int id)
        {
            try
            {
                await _bankUserDataOps.DeleteBankUserAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
