using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Simple_Digital_Wallet.Models;

namespace Web_API_Simple_Digital_Wallet.Dtos.Transaction
{
    public class AddtransDto
    {
        public string? SAddress { get; set; } = string.Empty;
        public string? RAddress { get; set; } = string.Empty;

        public double? Amount { get; set; }
        public TransactionType? Type { get; set; }
    }
}