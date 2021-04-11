using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BudgetSystemLab2.Entities;
using BudgetSystemLab2.Navigation;
using BudgetSystemLab2.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetSystemLab2.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private WalletService _service;
        public WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private DBUser _currentUser;
        public DelegateCommand AddWallet { get; }
     
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
          
            AddWallet = new DelegateCommand(AddNewWallet);
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            _currentUser = LoginedUser.User;
            //DBWallet w = new DBWallet(Guid.NewGuid(), "myWallet13", 13, "USD", "13");
            //DBWallet w1 = new DBWallet(Guid.NewGuid(), "myWallet133", 130, "EUR", "13");
            //_service.AddWalletsAsync(w);
            //_service.AddWalletsAsync(w1); 
            //DBWallet w2 = new DBWallet(Guid.NewGuid(), "myWallet11", 11, "USD", "11");
            //DBWallet w3 = new DBWallet(Guid.NewGuid(), "myWallet111", 110, "EUR", "11");
            //_service.AddWalletsAsync(w2);
            //_service.AddWalletsAsync(w3);
            List<DBWallet> _wallets = _service.GetUserWalletsAsync(_currentUser.Login);
            foreach (var wallet in _wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet, UpdateCurrentWallet, DeleteCurrentWallet));
            }
        }
        public void DeleteCurrentWallet(WalletDetailsViewModel wd)
        {
        
            _service.DeleteWalletsAsync(_currentWallet.WalletGuid());
            Wallets.Remove(wd);
            RaisePropertyChanged(nameof(CurrentWallet));
            RaisePropertyChanged(nameof(Wallets));
            MessageBox.Show("Wallet was deleted.");
        }
        public void UpdateCurrentWallet(WalletDetailsViewModel wd)
        {
            _service.UpdateWallet(_currentWallet.WalletGuid().ToString(), _currentWallet.Name, _currentWallet.Balance, _currentWallet.CurrencyEntrySelected, _currentWallet.WalletOwner());
            MessageBox.Show("Wallet was updated.");
        }
        public void AddNewWallet()
        {
            //_service.AddWalletsAsync(new DBWallet()) ;
            DBWallet w = new DBWallet(Guid.NewGuid(), "My wallet", 0, "UAH",_currentUser.Login);
            Wallets.Add(new WalletDetailsViewModel( w, UpdateCurrentWallet, DeleteCurrentWallet));
            RaisePropertyChanged(nameof(Wallets));
            MessageBox.Show("Wallet was added.");
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
