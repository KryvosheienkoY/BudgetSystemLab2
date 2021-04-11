using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetSystemLab2.Entities;
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
            DBWallet w = new DBWallet(Guid.NewGuid(), "myWallet13", 13, "USD", "13");
            DBWallet w1 = new DBWallet(Guid.NewGuid(), "myWallet133", 130, "EUR", "13");
            _service.AddWalletsAsync(w);
            _service.AddWalletsAsync(w1); 
            DBWallet w2 = new DBWallet(Guid.NewGuid(), "myWallet11", 11, "USD", "11");
            DBWallet w3 = new DBWallet(Guid.NewGuid(), "myWallet111", 110, "EUR", "11");
            _service.AddWalletsAsync(w2);
            _service.AddWalletsAsync(w3);
            List<DBWallet> _wallets = _service.GetUserWalletsAsync(_currentUser.Login);
            foreach (var wallet in _wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
            //List<DBWallet> _wallets = getWallets();
            //foreach (var wallet in _wallets)
            //{
            //    Wallets.Add(new WalletDetailsViewModel(wallet));
            //}
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
