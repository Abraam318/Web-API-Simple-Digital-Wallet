using Microsoft.EntityFrameworkCore;
using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Data;


namespace Web_API_Simple_Digital_Wallet.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByAddressAsync(string address)
        {
            return await _context.Users
                .Include(u => u.SentTransactions)
                .Include(u => u.ReceivedTransactions)
                .FirstOrDefaultAsync(u => u.Address == address);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Select(u => new User
            {
            Address = u.Address,
            Name = u.Name,
            Balance = u.Balance,
            Email = u.Email
            }).ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.Address);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Password = updatedUser.Password;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;

            await _context.SaveChangesAsync();
        }


        public async Task DeleteUserAsync(string address)
        {
            var user = await GetUserByAddressAsync(address);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
