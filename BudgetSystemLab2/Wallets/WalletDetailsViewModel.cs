using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using BudgetSystemLab2;
using Prism.Mvvm;
using System.Diagnostics;

namespace BudgetSystemLab2.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private Wallet _wallet;

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
        //public string Currency
        //{
        //    get
        //    {
        //        return _wallet.Currency.ToString("g");
        //    }
        //    set
        //    {
        //        //UAH is default currency
        //        if (value.Equals("EUR"))
        //            _wallet.Currency = Enums.Currency.EUR; 
        //        else if (value.Equals("USD"))
        //            _wallet.Currency = Enums.Currency.USD;
        //        else
        //            _wallet.Currency = Enums.Currency.UAH;
        //        RaisePropertyChanged(nameof(DisplayName));
        //    }
        //}
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


       // private ObservableCollection<CurrencyEntry> _currencies;

        public ObservableCollection<string> CurrencyEntries
        {
            get {
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
                _currencySelected = _wallet.Currency.ToString("g");
                Trace.WriteLine("cur - " + _wallet.Currency.ToString("g"));
               // Console.WriteLine("cur - " + _wallet.Currency.ToString("g"));
                return _currencySelected;
            }
            set
            {
                _currencySelected = value;
                //UAH is default currency
                if (value.Equals("EUR"))
                    _wallet.Currency = Enums.Currency.EUR;
                else if (value.Equals("USD"))
                    _wallet.Currency = Enums.Currency.USD;
                else
                    _wallet.Currency = Enums.Currency.UAH;
            }
        }


        public string DisplayName
        {
            get
            {
                var _cur = _wallet.Currency.ToString("g");
                return $"{_wallet.Name} ({_wallet.Balance} {_cur})";
            }
        }

        public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
        }
    }
}
