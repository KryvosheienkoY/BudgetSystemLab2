using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BudgetSystemLab2.Entities;
using BudgetSystemLab2.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetSystemLab2.Transactions
{
    public class TransactionDetailsViewModel : BindableBase
    {
        private DBTransaction _transaction;
        private TransactionService _service;
        private bool _isTransactionEnabled = true;
        public bool IsTransactionEnabled
        {
            get
            {
                return _isTransactionEnabled;
            }
            set
            {
                _isTransactionEnabled = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand UpdateTransaction { get; }
        public DelegateCommand DeleteTransaction { get; }
        public Guid TransactionGuid() { return _transaction.Guid; }
        public Guid TransactionWallet() { return _transaction.WalletId; }
        public TransactionDetailsViewModel(DBTransaction transaction, TransactionService ws, Action<TransactionDetailsViewModel> DeleteCurrentTransaction)
        {
            _service = ws;
            _transaction = transaction;
            UpdateTransaction = new DelegateCommand(UpdateCurrentTransaction);
            DeleteTransaction = new DelegateCommand(async () => DeleteCurrentTransaction(this));
        }
        public async void UpdateCurrentTransaction()
        {
            if (!IsTransactionValid())
            {
                MessageBox.Show($"Fill transaction`s name field!");
                return;
            }

            try
            {
                IsTransactionEnabled = false;
                await _service.UpdateTransaction(TransactionGuid(), Sum, CurrencyEntrySelected, DateTime, Description, _transaction.UserId, _transaction.WalletId);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction update was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsTransactionEnabled = true;
            }
            MessageBox.Show($"Transaction was updated successfully!");
        }
        private bool IsTransactionValid()
        {
            //return !String.IsNullOrWhiteSpace(Sum);
            return true;
        }
       
        public decimal Sum
        {
            get
            {
                return _transaction.Sum;
            }
            set
            {
                _transaction.Sum = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
           public string Description
        {
            get
            {
                return _transaction.Description;
            }
            set
            {
                _transaction.Description = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
        
        public DateTime DateTime
        {
            get
            {
                return _transaction.DateTime;
            }
            set
            {
                _transaction.DateTime = value;
                RaisePropertyChanged();
            }
        }
        public class CurrencyEntry
        {
            public string Name { get; set; }

            public CurrencyEntry(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }


        public ObservableCollection<string> CurrencyEntries
        {
            get
            {
                ObservableCollection<string> _currencyList = new ObservableCollection<string>();
                _currencyList.Add("EUR");
                _currencyList.Add("USD");
                _currencyList.Add("UAH");
                return _currencyList;
            }
            set { }
        }

        private string _currencySelected;

        public string CurrencyEntrySelected
        {
            get
            {
                _currencySelected = _transaction.CurrencyOfTransaction;             
                // Console.WriteLine("cur - " + _transaction.Currency.ToString("g"));
                return _currencySelected;
            }
            set
            {
                _currencySelected = value;
                _transaction.CurrencyOfTransaction = _currencySelected;
            }
        }


        public string DisplayName
        {
            get
            {
                return $"{_transaction.Sum}";
            }
        }
        public DBTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }
    }
}
