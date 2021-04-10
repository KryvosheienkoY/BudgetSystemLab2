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
        private FileDataStorage<Wallet> _storage = new FileDataStorage<Wallet>();
        private static List<Wallet> Wallets = new List<Wallet>()
        {
            new Wallet() {Owner=Guid.NewGuid(), Name = "wal1",Currency= Enums.Currency.USD},
            new Wallet() {Owner=Guid.NewGuid(), Name = "wal2",Currency= Enums.Currency.UAH},
            new Wallet() {Owner=Guid.NewGuid(), Name = "wal3",Currency= Enums.Currency.EUR},
            new Wallet() {Owner=Guid.NewGuid(), Name = "wal4",Currency= Enums.Currency.USD},
        };

        public List<Wallet> GetWallets()
        {
            return Wallets.ToList();
        }
        public async Task<List<Wallet>> GetUserWalletsAsync(Guid userGuid)
        {
            List<Wallet> _allWallets = await _storage.GetAllAsync();
            List<Wallet> _userWallets = new List<Wallet>();
            foreach (Wallet w in _allWallets)
            {
                if (w.Owner == userGuid)
                    _userWallets.Add(w);
            }
            return _userWallets;
        }
        public async void addWalletsAsync(Wallet wallet)
        {
            _storage.AddOrUpdateAsync(wallet);
        }
    }
}
