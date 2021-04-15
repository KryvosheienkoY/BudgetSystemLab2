using BudgetSystemLab2.Entities;
using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Services
{
    public class WalletService
    {
        private FileDataStorage<DBWallet> _storage = new FileDataStorage<DBWallet>();
        public static DBWallet CurrentWallet;
        public async Task<List<DBWallet>> GetUserWalletsAsync(string userLogin)
        {
            List<DBWallet> _allWallets = null;
            List<DBWallet> _userWallets = new List<DBWallet>();
            try
            {
                _allWallets = await GetAllWalletsAsync();
                foreach (var w in _allWallets)
                {
                    if (w.Owner.Equals(userLogin))
                        _userWallets.Add(w);
                }
            }
            catch (Exception ex)
            {
                throw new  Exception (ex.Message);
            }
            return _userWallets;
        }

        public async Task<bool> AddWalletsAsync(DBWallet wallet)
        {
            if (String.IsNullOrWhiteSpace(wallet.Name) || wallet.Balance < 0)
                throw new ArgumentException("Name or Balance is Empty.");

            await _storage.AddOrUpdateAsync(wallet);
            return true;
        }
        public async Task<bool> DeleteWalletsAsync(Guid walletGuid)
        {
            await _storage.DeleteAsync(walletGuid);
            return true;
        }

        public async Task<List<DBWallet>> GetAllWalletsAsync()
        {
            var wallets = await _storage.GetAllAsync();
            return wallets;
        }
        public async Task<bool> UpdateWallet(string guid, string name, decimal balance, string currency, string owner, string description, List<DBTransaction> transactions)
        {
            DBWallet wallet = new DBWallet(Guid.Parse(guid), name, balance, currency, owner, description, transactions);
            await _storage.AddOrUpdateAsync(wallet);
            return true;
        }

        public List<DBWallet> GetAllWalletsSync()
        {
            var task = Task.Run(async () => await GetAllWalletsAsync());
            return task.Result;
        }
    }
}
