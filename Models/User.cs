using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Simple_Digital_Wallet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Balance { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public ICollection<Transaction>? SentTransactions { get; set; }
        public ICollection<Transaction>? ReceivedTransactions { get; set; }
    }
}