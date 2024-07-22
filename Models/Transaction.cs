using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Simple_Digital_Wallet.Models
{
    public class Transaction
    {
       public int Id { get; set; }
        public string SAddress { get; set; } = string.Empty;
        public string RAddress { get; set; } =string.Empty;
        public double Amount { get; set; }
        public TransactionType Type { get; set;} 
        public DateTime Timestamp { get; set; } = DateTime.Now;
        // Navigation properties
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
    }
}