using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using BudgetSystemLab2;
using Prism.Mvvm;
using System.Diagnostics;
using BudgetSystemLab2.Entities;
using Prism.Commands;

namespace BudgetSystemLab2.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private DBWallet _wallet;
        public DelegateCommand UpdateWallet { get; }
        public DelegateCommand DeleteWallet { get; }

        public WalletDetailsViewModel(DBWallet wallet, Action<WalletDetailsViewModel> UpdateCurrentWallet, Action<WalletDetailsViewModel> DeleteCurrentWallet)
        {
            _wallet = wallet;
            UpdateWallet = new DelegateCommand(async () => UpdateCurrentWallet(this));
            DeleteWallet = new DelegateCommand(async () => DeleteCurrentWallet(this));
        }

        public Guid WalletGuid() { return _wallet.Guid; }
        public string WalletOwner() { return _wallet.Owner; }
      

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal Balance
        {
            get
            {
                return _wallet.Balance;
            }
            set
            {
                _wallet.Balance = value;
                RaisePropertyChanged(nameof(DisplayName));
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
                _currencySelected = _wallet.Currency;
                Trace.WriteLine("cur - " + _wallet.Currency);
                // Console.WriteLine("cur - " + _wallet.Currency.ToString("g"));
                return _currencySelected;
            }
            set
            {
                _currencySelected = value;
                _wallet.Currency = _currencySelected;
            }
        }


        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name} ({_wallet.Balance})";
            }
        }

      
    }
}
