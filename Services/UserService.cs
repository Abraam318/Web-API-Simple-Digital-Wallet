using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Repositories;

namespace Web_API_Simple_Digital_Wallet.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // CRUD Operations
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByAddressAsync(string address)
        {
            return await _userRepository.GetUserByAddressAsync(address);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string address)
        {
            await _userRepository.DeleteUserAsync(address);
        }

        public async Task<bool> ValidateUserAsync(string address, string password)
        {
            var user = await GetUserByAddressAsync(address);
            if (user == null)
                return false;

            string hashedPassword = HashPassword(password);
            return user.Password == hashedPassword;
        }

        private string HashPassword(string password)
        {
            string hash = string.Empty;
            foreach (char c in password)
            {
                hash += (int)c + 5 * 2;
            }
            return hash;
        }
    }
}
