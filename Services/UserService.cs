using Web_API_Simple_Digital_Wallet.Models;
using Web_API_Simple_Digital_Wallet.Repositories;

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

        public async Task<User> GetUserByAddressAsync(string address)
        {
            return await _userRepository.GetUserByAddressAsync(address);
        }

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserProfileAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> SendMoneyAsync(string senderAddress, string receiverAddress, double amount)
        {
            var sender = await _userRepository.GetUserByAddressAsync(senderAddress);
            var receiver = await _userRepository.GetUserByAddressAsync(receiverAddress);

            if (sender == null || receiver == null || sender.Balance < amount)
            {
                return false;
            }

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

            await _transactionService.AddTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(sender);
            await _userRepository.UpdateUserAsync(receiver);

            return true;
        }

        public async Task<bool> DepositAsync(string address, double amount)
        {
            var user = await _userRepository.GetUserByAddressAsync(address);
            if (user == null)
            {
                return false;
            }

            user.Balance += amount;
            var transaction = new Transaction
            {
                SAddress = "System",
                RAddress = address,
                Amount = amount,
                Type = TransactionType.Deposit,
                Timestamp = DateTime.UtcNow
            };

            await _transactionService.AddTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<bool> WithdrawAsync(string address, double amount)
        {
            var user = await _userRepository.GetUserByAddressAsync(address);
            if (user == null || user.Balance < amount)
            {
                return false;
            }

            user.Balance -= amount;
            var transaction = new Transaction
            {
                SAddress = address,
                RAddress = "System",
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Timestamp = DateTime.UtcNow
            };

            await _transactionService.AddTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<bool> RequestMoneyAsync(string requesterAddress, string receiverAddress, double amount)
        {
            var requester = await _userRepository.GetUserByAddressAsync(requesterAddress);
            var receiver = await _userRepository.GetUserByAddressAsync(receiverAddress);

            if (requester == null || receiver == null)
            {
                return false;
            }

            var transaction = new Transaction
            {
                SAddress = requesterAddress,
                RAddress = receiverAddress,
                Amount = amount,
                Type = TransactionType.Request,
                Timestamp = DateTime.UtcNow
            };

            await _transactionService.AddTransactionAsync(transaction);
            return true;
        }
    }
}
