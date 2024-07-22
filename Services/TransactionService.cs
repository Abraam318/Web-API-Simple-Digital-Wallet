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
    }
}
