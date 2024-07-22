namespace Web_API_Simple_Digital_Wallet.DTOs
{
    public class TransactionRequestDto
    {
        public string SenderAddress { get; set; } = string.Empty;
        public string ReceiverAddress { get; set; } = string.Empty;
        public double Amount { get; set; } = 0;
    }
}
