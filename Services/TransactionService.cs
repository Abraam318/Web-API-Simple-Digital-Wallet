using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Repositories;

namespace Web_API_Simple_Digital_Wallet.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly UserRepository _userRepository;

        public TransactionService(TransactionRepository transactionRepository, UserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        // CRUD Operations
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
        }

        // Additional Methods
        public async Task<bool> SendMoneyAsync(string senderAddress, string receiverAddress, double amount)
        {
            var sender = await _userRepository.GetUserByAddressAsync(senderAddress);
            var receiver = await _userRepository.GetUserByAddressAsync(receiverAddress);

            if (sender == null || receiver == null)
                return false;

            if (sender.Balance < amount)
                return false;

            sender.Balance -= amount;
            receiver.Balance += amount;

            var transaction = new Transaction
            {
                SAddress = senderAddress,
                RAddress = receiverAddress,
                Amount = amount,
                Type = TransactionType.Send,
                Timestamp = DateTime.UtcNow
            };

            await _userRepository.UpdateUserAsync(sender);
            await _userRepository.UpdateUserAsync(receiver);
            await AddTransactionAsync(transaction);

            return true;
        }

        public async Task<bool> ReceiveMoneyAsync(string senderAddress, string receiverAddress, double amount)
        {
            return await SendMoneyAsync(senderAddress, receiverAddress, amount);
        }

        public async Task<bool> RequestMoneyAsync(string senderAddress, string receiverAddress, double amount)
        {
            var receiver = await _userRepository.GetUserByAddressAsync(receiverAddress);

            if (receiver == null)
                return false;

            var requestTransaction = new Transaction
            {
                SAddress = senderAddress,
                RAddress = receiverAddress,
                Amount = amount,
                Type = TransactionType.Request,
                Timestamp = DateTime.UtcNow
            };

            await AddTransactionAsync(requestTransaction);

            return true;
        }

        public async Task<bool> DepositAsync(string userAddress, double amount)
        {
            var user = await _userRepository.GetUserByAddressAsync(userAddress);

            if (user == null)
                return false;

            user.Balance += amount;

            var transaction = new Transaction
            {
                SAddress = "System",
                RAddress = userAddress,
                Amount = amount,
                Type = TransactionType.Deposit,
                Timestamp = DateTime.UtcNow
            };

            await _userRepository.UpdateUserAsync(user);
            await AddTransactionAsync(transaction);

            return true;
        }

        public async Task<bool> WithdrawAsync(string userAddress, double amount)
        {
            var user = await _userRepository.GetUserByAddressAsync(userAddress);

            if (user == null || user.Balance < amount)
                return false;

            user.Balance -= amount;

            var transaction = new Transaction
            {
                SAddress = userAddress,
                RAddress = "System",
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Timestamp = DateTime.UtcNow
            };

            await _userRepository.UpdateUserAsync(user);
            await AddTransactionAsync(transaction);

            return true;
        }
    }
}
