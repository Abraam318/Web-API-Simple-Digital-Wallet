using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_API_Simple_Digital_Wallet.Models
{
    public class User
    {
        [Key]
        public string Address { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public double? Balance { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    }
}
