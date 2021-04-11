using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private DBUser _currentUser;
        public DelegateCommand DeleteWallet { get; }
        public DelegateCommand AddWallet { get; }
        public DelegateCommand UpdateWallet { get; }
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
            DeleteWallet = new DelegateCommand(DeleteCurrentWallet);
            UpdateWallet = new DelegateCommand(UpdateCurrentWallet);
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
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
        }
        public void DeleteCurrentWallet()
        {
            _service.DeleteWalletsAsync(_currentWallet.WalletGuid());

        }
        public void UpdateCurrentWallet()
        {
            _service.UpdateWallet(_currentWallet.WalletGuid().ToString(), _currentWallet.Name, _currentWallet.Balance, _currentWallet.CurrencyEntrySelected, _currentWallet.WalletOwner());

        }
        public void AddNewWallet()
        {
            //_service.AddWalletsAsync(new DBWallet()) ;

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
