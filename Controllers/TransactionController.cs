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

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
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

        [HttpPost("send")]
        public async Task<ActionResult<bool>> SendMoney(string senderAddress, string receiverAddress, double amount)
        {
            var success = await _transactionService.SendMoneyAsync(senderAddress, receiverAddress, amount);
            if (!success)
                return BadRequest("Transaction failed.");
            return Ok(true);
        }

        [HttpPost("receive")]
        public async Task<ActionResult<bool>> ReceiveMoney(string senderAddress, string receiverAddress, double amount)
        {
            var success = await _transactionService.ReceiveMoneyAsync(senderAddress, receiverAddress, amount);
            if (!success)
                return BadRequest("Transaction failed.");
            return Ok(true);
        }

        [HttpPost("request")]
        public async Task<ActionResult<bool>> RequestMoney(string senderAddress, string receiverAddress, double amount)
        {
            var success = await _transactionService.RequestMoneyAsync(senderAddress, receiverAddress, amount);
            if (!success)
                return BadRequest("Request failed.");
            return Ok(true);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<bool>> Deposit(string userAddress, double amount)
        {
            var success = await _transactionService.DepositAsync(userAddress, amount);
            if (!success)
                return BadRequest("Deposit failed.");
            return Ok(true);
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<bool>> Withdraw(string userAddress, double amount)
        {
            var success = await _transactionService.WithdrawAsync(userAddress, amount);
            if (!success)
                return BadRequest("Withdrawal failed.");
            return Ok(true);
        }
    }
}
