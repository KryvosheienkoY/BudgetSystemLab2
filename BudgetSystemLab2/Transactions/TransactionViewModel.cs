using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BudgetSystemLab2.Entities;
using BudgetSystemLab2.Navigation;
using BudgetSystemLab2.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetSystemLab2.Transactions
{
    class TransactionViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private TransactionService _service;
        public TransactionDetailsViewModel _currentTransaction;
        private DBUser _currentUser;
        private bool _isTransactionPanelEnabled = true;

        public bool IsTransactionPanelEnabled
        {
            get
            {
                return _isTransactionPanelEnabled;
            }
            set
            {
                _isTransactionPanelEnabled = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddTransaction { get; }
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set; }
        public TransactionDetailsViewModel CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }
            set
            {
                _currentTransaction = value;
                RaisePropertyChanged();
            }
        }

        public TransactionViewModel()
        {

            AddTransaction = new DelegateCommand(AddNewTransaction);
            _service = new TransactionService();
            Transactions = new ObservableCollection<TransactionDetailsViewModel>();
            _currentUser = LoginedUser.User;
            AddTransactionsView();
        }

        private async void AddTransactionsView()
        {
            List<DBTransaction> _transactions = null;
            try
            {
                IsTransactionPanelEnabled = false;
                _transactions = await _service.GetWalletTransactionsAsync(WalletService.CurrentWallet.Guid);
                foreach (var tr in _transactions)
                {
                    Transactions.Add(new TransactionDetailsViewModel(tr, _service, DeleteCurrentTransaction));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transactions loading was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsTransactionPanelEnabled = true;
            }
        }

        public async void AddNewTransaction()
        {
            try
            {
                IsTransactionPanelEnabled = false;
                DBTransaction w = new DBTransaction(0, "UAH", DateTime.Now, "", Guid.NewGuid(),_currentTransaction.TransactionWallet());
                await _service.AddTransactionAsync(w);
                Transactions.Add(new TransactionDetailsViewModel(w, _service, DeleteCurrentTransaction));
                RaisePropertyChanged(nameof(Transactions));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction add was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsTransactionPanelEnabled = true;
            }
            MessageBox.Show($"Transaction was added successfully!");
        }

        public async void DeleteCurrentTransaction(TransactionDetailsViewModel wd)
        {
            try
            {
                IsTransactionPanelEnabled = false;
                await _service.DeleteTransactionAsync(_currentTransaction.TransactionWallet());
                Transactions.Remove(wd);
                RaisePropertyChanged(nameof(CurrentTransaction));
                RaisePropertyChanged(nameof(Transactions));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction delete was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsTransactionPanelEnabled = true;
            }
            MessageBox.Show($"Transaction was deleted successfully!");
        }

        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Transactions;
            }
        }
        public void ClearSensitiveData()
        {
        }
    }
}
