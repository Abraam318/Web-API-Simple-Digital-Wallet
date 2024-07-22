using Microsoft.AspNetCore.Mvc;
using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Services;

namespace Web_API_Simple_Digital_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly UserService _userService;

        public TransactionController(TransactionService transactionService, UserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }
    

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            await _transactionService.AddTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest();

            await _transactionService.UpdateTransactionAsync(transaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }

    }
}
