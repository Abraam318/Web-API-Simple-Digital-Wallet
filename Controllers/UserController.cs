using Microsoft.AspNetCore.Mvc;
using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Services;
using Web_API_Simple_Digital_Wallet.DTOs;
using Web_API_Simple_Digital_Wallet.Dtos;

namespace Web_API_Simple_Digital_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;

        public UserController(UserService userService, TransactionService transactionService)
        {
            _userService = userService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var getDataDto = users.Select( User => new GetDataDto
            {
                Name = User.Name,
                Address = User.Address,
                PhoneNumber = User.PhoneNumber,
                Email = User.Email,
                Balance = User.Balance
            }).ToList();

            return Ok(getDataDto);
        }

        [HttpGet("{address}")]
        public async Task<ActionResult<User>> GetUserByAddress(string address)
        {
            var user = await _userService.GetUserByAddressAsync(address);
            if (user == null)
                return NotFound();

            var getDataDto = new GetDataDto  {
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                Balance = user.Balance
            };
            return Ok(getDataDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var isValid = await _userService.ValidateUserAsync(userLoginDto.Address, userLoginDto.Password);

                return isValid?  Ok("Login successful.") : Unauthorized("Invalid address or password.");
        }


        [HttpPost("signup")]
        public async Task<ActionResult> AddUser([FromBody] UserSignupDto userSignupDto)
        {
            var user = new User
            {
                Address = userSignupDto.Address,
                Password = HashPassword(userSignupDto.Password),
                Name = userSignupDto.Name,
                Email = userSignupDto.Email,
                PhoneNumber = userSignupDto.PhoneNumber
            };

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserByAddress), new { address = user.Address }, user);
        }

        [HttpPut("{address}")]
        public async Task<ActionResult> UpdateUser(string address, [FromBody] UserEditDto userEditDto)
        {
            var existingUser = await _userService.GetUserByAddressAsync(address);
            if (existingUser == null)
                return NotFound();

            if (!string.IsNullOrEmpty(userEditDto.Name))
                existingUser.Name = userEditDto.Name;
            if (!string.IsNullOrEmpty(userEditDto.Password))
                existingUser.Password = HashPassword(userEditDto.Password);
            if (!string.IsNullOrEmpty(userEditDto.Email))
                existingUser.Email = userEditDto.Email;
            if (!string.IsNullOrEmpty(userEditDto.PhoneNumber))
                existingUser.PhoneNumber = userEditDto.PhoneNumber;

            await _userService.UpdateUserAsync(existingUser);
            return NoContent();
        }

        [HttpDelete("{address}")]
        public async Task<ActionResult> DeleteUser(string address)
        {
            await _userService.DeleteUserAsync(address);
            return NoContent();
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendMoney([FromBody] TransactionRequestDto transactionRequestDto)
        {
            var success = await _userService.SendMoneyAsync(transactionRequestDto.SenderAddress, transactionRequestDto.ReceiverAddress, transactionRequestDto.Amount);
            if (!success)
                return BadRequest("Transaction failed.");
            return Ok(true);
        }

        [HttpPost("receive")]
        public async Task<ActionResult> ReceiveMoney([FromBody] TransactionRequestDto transactionRequestDto)
        {
            var success = await _userService.ReceiveMoneyAsync(transactionRequestDto.SenderAddress, transactionRequestDto.ReceiverAddress, transactionRequestDto.Amount);
            if (!success)
                return BadRequest("Transaction failed.");
            return Ok(true);
        }

        [HttpPost("request")]
        public async Task<ActionResult> RequestMoney([FromBody] TransactionRequestDto transactionRequestDto)
        {
            var success = await _userService.RequestMoneyAsync(transactionRequestDto.SenderAddress, transactionRequestDto.ReceiverAddress, transactionRequestDto.Amount);
            if (!success)
                return BadRequest("Request failed.");
            return Ok(true);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit([FromBody] TransactionRequestDto transactionRequestDto)
        {
            var success = await _userService.DepositAsync(transactionRequestDto.ReceiverAddress, transactionRequestDto.Amount);
            if (!success)
                return BadRequest("Deposit failed.");
            return Ok(true);
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult> Withdraw([FromBody] TransactionRequestDto transactionRequestDto)
        {
            var success = await _userService.WithdrawAsync(transactionRequestDto.ReceiverAddress, transactionRequestDto.Amount);
            if (!success)
                return BadRequest("Withdrawal failed.");
            return Ok(true);
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            string hash = string.Empty;
            foreach (char c in password)
            {
                hash += (int)c + 5 * 2;
            }
            return hash;
        }
    }
}
