using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Simple_Digital_Wallet.Dtos
{
    public class GetDataDto
    {
        public string? Address { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public double? Balance {get; set;} = 0;
    }
}