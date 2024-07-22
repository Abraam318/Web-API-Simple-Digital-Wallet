using Microsoft.AspNetCore.Mvc;
using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Services;

namespace Web_API_Simple_Digital_Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{address}")]
        public async Task<ActionResult<User>> GetUserByAddress(string address)
        {
            var user = await _userService.GetUserByAddressAsync(address);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {
            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserByAddress), new { address = user.Address }, user);
        }

        [HttpPut("{address}")]
        public async Task<ActionResult> UpdateUser(string address, [FromBody] User user)
        {
            if (address != user.Address)
                return BadRequest();

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{address}")]
        public async Task<ActionResult> DeleteUser(string address)
        {
            await _userService.DeleteUserAsync(address);
            return NoContent();
        }
    }
}
