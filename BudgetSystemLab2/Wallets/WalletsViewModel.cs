using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private DBUser _currentUser;
        private bool _isWalletPanelEnabled = true;

        public bool IsWalletPanelEnabled
        {
            get
            {
                return _isWalletPanelEnabled;
            }
            set
            {
                _isWalletPanelEnabled = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddWallet { get; }
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                WalletService.CurrentWallet = _currentWallet.Wallet;
                RaisePropertyChanged();
            }
        }

        public WalletsViewModel()
        {

            AddWallet = new DelegateCommand(AddNewWallet);
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            _currentUser = LoginedUser.User;
            AddWalletsView();
        }

        private async void AddWalletsView()
        {
            List<DBWallet> _wallets = null;
            try
            {
                IsWalletPanelEnabled = false;
                 
                _wallets = await _service.GetUserWalletsAsync(_currentUser.Login);
                foreach (var wallet in _wallets)
                {
                    Wallets.Add(new WalletDetailsViewModel(wallet, _service, DeleteCurrentWallet));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wallets loading was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletPanelEnabled = true;
            }
        }

        public async void AddNewWallet()
        {
            try
            {
                IsWalletPanelEnabled = false;
                DBWallet w = new DBWallet(Guid.NewGuid(), "My wallet", 0, "UAH", _currentUser.Login);
                await _service.AddWalletsAsync(w);
                Wallets.Add(new WalletDetailsViewModel(w, _service, DeleteCurrentWallet));
                RaisePropertyChanged(nameof(Wallets));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wallet add was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletPanelEnabled = true;
            }
            MessageBox.Show($"Wallet was added successfully!");
        }

        public async void DeleteCurrentWallet(WalletDetailsViewModel wd)
        {
            try
            {
                IsWalletPanelEnabled = false;
                await _service.DeleteWalletsAsync(_currentWallet.WalletGuid());
                Wallets.Remove(wd);
                RaisePropertyChanged(nameof(CurrentWallet));
                RaisePropertyChanged(nameof(Wallets));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wallet delete was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletPanelEnabled = true;
            }
            MessageBox.Show($"Wallet was deleted successfully!");
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
