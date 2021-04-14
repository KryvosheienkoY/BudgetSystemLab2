using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Entities
{
  public class DBTransaction : EntityBase,IStorable
    {
        private Guid _guid;
        private Guid _userId;
        private Guid _walletId;
        private decimal _sum;
        private string _description;
        private DateTime _date;
        private string _currency;

        public Guid Guid { get => _guid; set => _guid = value; }
        public Guid UserId { get => _userId; set => _userId = value; }
        public Guid WalletId { get => _walletId; set => _walletId = value; }
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = (value.CompareTo(decimal.Zero) > 0) ? value : _sum;
            }
        }
        public DateTime DateTime { get => _date; set => _date = value; }
        public string Description { get => _description; set => _description = value; }
        public string CurrencyOfTransaction { get => _currency; set => _currency = value; }

        public DBTransaction(Guid userId)
        {
            _guid = Guid.NewGuid();
            _userId = userId;
        }
        public DBTransaction( decimal sum, string currency, DateTime dateTime, string description, Guid userId, Guid walletId)
        {
            _sum = sum;
            _currency = currency;
            _date = dateTime;
            _description = description;
            _userId = userId;
            _walletId = walletId;
        }
        public override bool Validate()
        {
            return (Guid != Guid.Empty) &&  _date != default(DateTime) && _description != null && (UserId != Guid.Empty);
        }
    }
}