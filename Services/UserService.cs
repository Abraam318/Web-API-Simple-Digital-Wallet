using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Repositories;
using Web_API_Simple_Digital_Wallet.Services;

namespace Web_API_Simple_Digital_Wallet.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly TransactionService _transactionService;

        public UserService(UserRepository userRepository, TransactionService transactionService)
        {
            _userRepository = userRepository;
            _transactionService = transactionService;
        }

        // CRUD Operations
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByAddressAsync(string address)
        {
            return await _userRepository.GetUserByAddressAsync(address);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string address)
        {
            await _userRepository.DeleteUserAsync(address);
        }

        public async Task<bool> ValidateUserAsync(string address, string password)
        {
            var user = await GetUserByAddressAsync(address);
            if (user == null)
                return false;

            string hashedPassword = HashPassword(password);
            return user.Password == hashedPassword;
        }

        private string HashPassword(string password)
        {
            string hash = string.Empty;
            foreach (char c in password)
            {
                hash += (int)c + 5 * 2;
            }
            return hash;
        }

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
            await _transactionService.AddTransactionAsync(transaction);

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

            await _transactionService.AddTransactionAsync(requestTransaction);

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
            await _transactionService.AddTransactionAsync(transaction);

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
            await _transactionService.AddTransactionAsync(transaction);

            return true;
        }
    }
}
