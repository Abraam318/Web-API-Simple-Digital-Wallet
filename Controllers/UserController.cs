using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Services;

namespace Web_API_Simple_Digital_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{address}")]
        public async Task<ActionResult<User>> GetUserByAddress(string address)
        {
            var user = await _userService.GetUserByAddressAsync(address);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(User user)
        {
            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserByAddress), new { address = user.Address }, user);
        }

        [HttpPut("{address}")]
        public async Task<IActionResult> UpdateProfile(string address, User user)
        {
            var existingUser = await _userService.GetUserByAddressAsync(address);
            if (existingUser == null)
            {
                return NotFound();
            }

            user.Address = address; // Ensure the address matches
            await _userService.UpdateUserProfileAsync(user);
            return NoContent();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMoney([FromBody] TransactionRequest request)
        {
            var result = await _userService.SendMoneyAsync(request.SAddress, request.RAddress, request.Amount);
            if (!result)
            {
                return BadRequest("Transaction failed.");
            }
            return Ok("Transaction successful.");
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            var result = await _userService.DepositAsync(request.Address, request.Amount);
            if (!result)
            {
                return BadRequest("Deposit failed.");
            }
            return Ok("Deposit successful.");
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequest request)
        {
            var result = await _userService.WithdrawAsync(request.Address, request.Amount);
            if (!result)
            {
                return BadRequest("Withdrawal failed.");
            }
            return Ok("Withdrawal successful.");
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestMoney([FromBody] RequestMoneyRequest request)
        {
            var result = await _userService.RequestMoneyAsync(request.RequesterAddress, request.ReceiverAddress, request.Amount);
            if (!result)
            {
                return BadRequest("Request failed.");
            }
            return Ok("Request sent successfully.");
        }
    }

    public class TransactionRequest
    {
        public string SAddress { get; set; } = string.Empty;
        public string RAddress { get; set; } = string.Empty;
        public double Amount { get; set; }
    }

    public class DepositRequest
    {
        public string Address { get; set; } = string.Empty;
        public double Amount { get; set; }
    }

    public class WithdrawalRequest
    {
        public string Address { get; set; } = string.Empty;
        public double Amount { get; set; }
    }

    public class RequestMoneyRequest
    {
        public string RequesterAddress { get; set; } = string.Empty;
        public string ReceiverAddress { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
