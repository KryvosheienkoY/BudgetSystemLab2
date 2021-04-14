using BudgetSystemLab2.Entities;
using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Services
{
   public class TransactionService
    {
        private FileDataStorage<DBTransaction> _storage = new FileDataStorage<DBTransaction>();

        public async Task<List<DBTransaction>> GetWalletTransactionsAsync(Guid walletId)
        {
            List<DBTransaction> _allTransactions = null;
            List<DBTransaction> _walletTransactions = new List<DBTransaction>();
            try
            {
                _allTransactions = await GetAllTransactionsAsync();
                foreach (var tr in _allTransactions)
                {
                    if (tr.WalletId.Equals(walletId))
                        _walletTransactions.Add(tr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _walletTransactions;
        }

        public async Task<bool> AddTransactionAsync(DBTransaction tr)
        {
            if (!tr.IsValid)
                throw new ArgumentException("Transaction isn`t valid.");

            await _storage.AddOrUpdateAsync(tr);
            return true;
        }
        public async Task<bool> DeleteTransactionAsync(Guid transactionGuid)
        {
            await _storage.DeleteAsync(transactionGuid);
            return true;
        }

        public async Task<List<DBTransaction>> GetAllTransactionsAsync()
        {
            var tr = await _storage.GetAllAsync();
            return tr;
        }
        public async Task<bool> UpdateTransaction(Guid guid, decimal sum, string currency, DateTime dateTime, string description, Guid userId, Guid walletId)
        {
            DBTransaction wallet = new DBTransaction(guid, sum, currency, dateTime, description, userId, walletId);
            await _storage.AddOrUpdateAsync(wallet);
            return true;
        }

        public List<DBTransaction> GetAllTransactionsSync()
        {
            var task = Task.Run(async () => await GetAllTransactionsAsync());
            return task.Result;
        }
    }
}
