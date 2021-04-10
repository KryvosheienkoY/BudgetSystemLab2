using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetSystemLab2.Navigation;
using BudgetSystemLab2.Services;
using Prism.Mvvm;

namespace BudgetSystemLab2.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private WalletService _service;
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private DBUser _currentUser;
        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }

        public WalletsViewModel()
        {
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            _currentUser = LoginedUser.User;
            Wallet w = new Wallet() { Name = "myWallet", Currency = Enums.Currency.USD, Owner = Guid.Parse("0666b686-5491-42b4-992d-4d879dbf7c25") };
            Wallet w1 = new Wallet() { Name = "myWallet1", Currency = Enums.Currency.EUR, Owner = Guid.Parse("0666b686-5491-42b4-992d-4d879dbf7c25") };
            // _service.addWalletsAsync(w);
            // _service.addWalletsAsync(w1);
            List<Wallet> _wallets = getWallets();
            foreach (var wallet in _wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
        }

        public List<Wallet> getWallets()
        {
            Task<List<Wallet>> task =  _service.GetUserWalletsAsync(_currentUser.Guid);
            //List<Wallet> _wallets = new List<Wallet>();
            //Task continuation = task.ContinueWith(t =>
            //{
            //    Console.WriteLine("Result: " + t.Result);
            //     _wallets = t.Result;
            //});
            //continuation.Wait();
            return task.Result;
        }

        public MainNavigatableTypes Type 
        {
            get
            {
                return MainNavigatableTypes.Wallets;
            }
        }
        public void ClearSensitiveData()
        {
            
        }
    }
}
