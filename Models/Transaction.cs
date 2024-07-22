using System;
using System.ComponentModel.DataAnnotations;

namespace Web_API_Simple_Digital_Wallet.Models
{
    public enum TransactionType
    {
        Send,
        Receive,
        Deposit,
        Request,
        Withdrawal
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string SAddress { get; set; } = string.Empty;
        public string RAddress { get; set; } = string.Empty;
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
    }
}
