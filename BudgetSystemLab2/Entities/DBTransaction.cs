using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Entities
{
    public class DBTransaction : EntityBase, IStorable
    {
        //private Guid _guid;
        //private Guid _userId;
        //private Guid _walletId;
        //private decimal _sum;
        //private string _description;
        //private DateTime _date;
        //private string _currency;

        public Guid Guid { get; set; }
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public decimal Sum {get; set;}
        public string CurrencyOfTransaction { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        //public string CurrencyOfTransaction { get => _currency; set => _currency = value; }  public Guid Guid { get => _guid; set => _guid = value; }
        //public Guid UserId { get => _userId; set => _userId = value; }
        //public Guid WalletId { get => _walletId; set => _walletId = value; }
        //public decimal Sum
        //{
        //    get => _sum;
        //    set
        //    {
        //        _sum = (value.CompareTo(decimal.Zero) > 0) ? value : _sum;
        //    }
        //}
        //public DateTime DateTime { get => _date; set => _date = value; }
        //public string Description { get => _description; set => _description = value; }
        //public string CurrencyOfTransaction { get => _currency; set => _currency = value; }

        //public DBTransaction(Guid userId)
        //{
        //    _guid = Guid.NewGuid();
        //    _userId = userId;
        //}
        [JsonConstructor]
        public DBTransaction(Guid guid, decimal sum, string currencyOfTransaction, DateTime dateTime, string description, Guid userId, Guid walletId)
        {
            Guid = guid;
            Sum = sum;
            CurrencyOfTransaction = currencyOfTransaction;
            DateTime = dateTime;
            Description = description;
            UserId = userId;
            WalletId = walletId;
            //_guid = guid;
            //_sum = sum;
            //_currency = currency;
            //_date = dateTime;
            //_description = description;
            //_userId = userId;
            //_walletId = walletId;
        }
        public override bool Validate()
        {
            //return (Guid != Guid.Empty) &&  _date != default(DateTime) && _description != null && (UserId != Guid.Empty);
            return (true);
        }
    }
}