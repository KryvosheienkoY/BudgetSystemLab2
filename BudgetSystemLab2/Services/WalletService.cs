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

        public List<DBWallet> GetUserWalletsAsync(string userLogin)
        {
            List<DBWallet> _allWallets = GetAllWalletsSync();
            List<DBWallet> _userWallets = new List<DBWallet>();
            foreach (DBWallet w in _allWallets)
            {
                if (w.Owner.Equals(userLogin))
                    _userWallets.Add(w);
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
        public void UpdateWallet(string guid, string name, decimal balance, string currency, string owner)
        {
            DBWallet wallet = new DBWallet(Guid.Parse(guid), name, balance, currency, owner);
            var task = Task.Run(async () => await _storage.AddOrUpdateAsync(wallet));
        }
        public List<DBWallet> GetAllWalletsSync()
        {
            var task = Task.Run(async () => await GetAllWalletsAsync());
            return task.Result;
        }
    }
}
