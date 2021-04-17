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
using BudgetSystemLab2.Transactions;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetSystemLab2.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private WalletService _serviceWallet;
        private TransactionService _serviceTransaction;
        public WalletDetailsViewModel _currentWalletView;
        public TransactionDetailsViewModel _currentTransaction;
        private DBWallet _currentWallet;
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
                _isWalletPanelEnabled = true;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddWallet { get; }
        public DelegateCommand AddTransaction { get; }
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set; }
        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWalletView;
            }
            set
            {
                _currentTransaction = null;
                _currentWalletView = value;
                Transactions = new ObservableCollection<TransactionDetailsViewModel>();

                if (_currentWalletView != null)
                {
                    WalletService.CurrentWallet = _currentWalletView.Wallet;
                    _currentWallet = _currentWalletView.Wallet;
                    AddTransactionsView();
                }

                RaisePropertyChanged();

                OnPropertyChanged(nameof(CurrentWallet));
                OnPropertyChanged(nameof(CurrentTransaction));
                OnPropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(CurrentWallet));
                RaisePropertyChanged(nameof(CurrentTransaction));
                RaisePropertyChanged(nameof(Transactions));
            }
        }
        public TransactionDetailsViewModel CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }
            set
            {
                _currentWalletView = null;
                _currentTransaction = value;
                RaisePropertyChanged();
                OnPropertyChanged(nameof(CurrentWallet));
                OnPropertyChanged(nameof(CurrentTransaction));
                RaisePropertyChanged(nameof(CurrentWallet));
                RaisePropertyChanged(nameof(CurrentTransaction));
            }
        }
        public WalletsViewModel()
        {
            AddWallet = new DelegateCommand(AddNewWallet);
            AddTransaction = new DelegateCommand(AddNewTransaction);
            _serviceWallet = new WalletService();
            _serviceTransaction = new TransactionService();
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

                _wallets = await _serviceWallet.GetUserWalletsAsync(_currentUser.Login);
                foreach (var wallet in _wallets)
                {
                    Wallets.Add(new WalletDetailsViewModel(wallet, _serviceWallet, DeleteCurrentWallet));
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
                DBWallet w = new DBWallet(Guid.NewGuid(), "My wallet", 0, "UAH", _currentUser.Login, "Here you can add description.", new List<DBTransaction>());
                await _serviceWallet.AddWalletsAsync(w);
                Wallets.Add(new WalletDetailsViewModel(w, _serviceWallet, DeleteCurrentWallet));
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
        private async void AddTransactionsView()
        {
            List<DBTransaction> _transactions = null;
            try
            {
                IsWalletPanelEnabled = false;
                _transactions = await _serviceTransaction.GetWalletTransactionsAsync(WalletService.CurrentWallet.Guid);
                foreach (var tr in _transactions)
                {
                    Transactions.Add(new TransactionDetailsViewModel(tr, _serviceTransaction, _serviceWallet, _currentWallet, DeleteCurrentTransaction));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transactions loading was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletPanelEnabled = true;
            }
        }

        public async void AddNewTransaction()
        {
            try
            {
                IsWalletPanelEnabled = false;
                DBTransaction tr = new DBTransaction(Guid.NewGuid(), 0m, "UAH", DateTime.Now, "default", Guid.NewGuid(), _currentWallet.Guid);
                await _serviceTransaction.AddTransactionAsync(tr); 
                Transactions.Add(new TransactionDetailsViewModel(tr, _serviceTransaction, _serviceWallet,_currentWallet, DeleteCurrentTransaction));

                _currentWallet.AddTransaction(_currentUser.Guid, tr);
                await _serviceWallet.UpdateWallet(_currentWallet.Guid.ToString(), _currentWallet.Name, _currentWallet.Balance, _currentWallet.Currency, _currentWallet.Owner, _currentWallet.Description, _currentWallet.Transactions);

                RaisePropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(CurrentWallet));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction add was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletPanelEnabled = true;
            }
            MessageBox.Show($"Transaction was added successfully!");
        }

        public async void DeleteCurrentTransaction(TransactionDetailsViewModel wd)
        {
            try
            {
                IsWalletPanelEnabled = false;

                _currentWallet.DeleteTransaction( wd.Transaction);
                await _serviceWallet.UpdateWallet(_currentWallet.Guid.ToString(), _currentWallet.Name, _currentWallet.Balance, _currentWallet.Currency, _currentWallet.Owner, _currentWallet.Description, _currentWallet.Transactions);

                await _serviceTransaction.DeleteTransactionAsync(_currentTransaction.TransactionGuid());
                Transactions.Remove(wd);

                RaisePropertyChanged(nameof(CurrentTransaction));
                RaisePropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(CurrentWallet));
                RaisePropertyChanged(nameof(Wallets));
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction delete was failed: {ex.Message}");
                return;
            }
            finally
            {       
                IsWalletPanelEnabled = true;
            }
            MessageBox.Show($"Transaction was deleted successfully!");
        }
        public async void DeleteCurrentWallet(WalletDetailsViewModel wd)
        {
            try
            {
                IsWalletPanelEnabled = false;
                await _serviceWallet.DeleteWalletsAsync(_currentWallet.Guid);
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
        public void Update()
        {
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            AddWalletsView();
            OnPropertyChanged(nameof(Wallets));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
