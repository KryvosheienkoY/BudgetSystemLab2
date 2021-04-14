using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using BudgetSystemLab2;
using Prism.Mvvm;
using System.Diagnostics;
using BudgetSystemLab2.Entities;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows;
using BudgetSystemLab2.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BudgetSystemLab2.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private DBWallet _wallet;
        private WalletService _service;
        private bool _isWalletEnabled = true;
        public bool IsWalletEnabled
        {
            get
            {
                return _isWalletEnabled;
            }
            set
            {
                _isWalletEnabled = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand UpdateWallet { get; }
        public DelegateCommand DeleteWallet { get; }
        public Guid WalletGuid() { return _wallet.Guid; }
        public string WalletOwner() { return _wallet.Owner; }
        public WalletDetailsViewModel(DBWallet wallet, WalletService ws, Action<WalletDetailsViewModel> DeleteCurrentWallet)
        {
            _service = ws;
            _wallet = wallet;
            UpdateWallet = new DelegateCommand(UpdateCurrentWallet);
            //DeleteWallet = new DelegateCommand(DeleteCurrentWallet);
            DeleteWallet = new DelegateCommand(async () => DeleteCurrentWallet(this));
        }
        public async void UpdateCurrentWallet()
        {
            if (!IsWalletValid())
            {
                MessageBox.Show($"Fill wallet`s name field!");
                return;
            }
               
            try
            {
               IsWalletEnabled = false;
               await _service.UpdateWallet(_wallet.Guid.ToString(), Name, Balance, CurrencyEntrySelected, _wallet.Owner);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wallet update was failed: {ex.Message}");
                return;
            }
            finally
            {
                IsWalletEnabled = true;
            }
            MessageBox.Show($"Wallet was updated successfully!");
        }
        private bool IsWalletValid()
        {
            return !String.IsNullOrWhiteSpace(Name);
        }
        //public async void DeleteCurrentWallet()
        //{
        //    try
        //    {
        //        //IsWalletEnabled = false;
        //        await _service.DeleteWalletsAsync(_wallet.Guid);
        //        RaisePropertyChanged(nameof(_wallet));
        //        RaisePropertyChanged(nameof(Wallets));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Wallet delete was failed: {ex.Message}");
        //        return;
        //    }
        //    finally
        //    {
        //        IsWalletEnabled = true;
        //    }
        //    MessageBox.Show($"Wallet was deleted successfully!");
        //}

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
        public DBWallet Wallet
        {
            get
            {
                return _wallet;
            }
        }
    }
}
