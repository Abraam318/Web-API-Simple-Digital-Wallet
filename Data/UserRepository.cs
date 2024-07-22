using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Simple_Digital_Wallet.Data
{
    public class UserRepository
    {
        private readonly DigitalWalletContext _context;

        public UserRepository(DigitalWalletContext context)
        {
            _context = context;
        }

        public User GetUserByAddress(string address)
        {
            return _context.Users.SingleOrDefault(u => u.Address == address);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public bool UserExists(string address)
        {
            return _context.Users.Any(u => u.Address == address);
        }
    }
}