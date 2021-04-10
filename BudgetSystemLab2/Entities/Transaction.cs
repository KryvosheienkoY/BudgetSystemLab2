using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public class Transaction : EntityBase
    {
      //  private static int InstanceCount;
      //  private int _id;
        private Guid _guid;
        private Guid _userId;
        private decimal _sum;
        private Category _category;
        private string _description;
        private DateTime _date;
        private Enums.Currency _currency;
        public string _filePath;

        public Guid Guid { get => _guid; private set => _guid = value; }
        public Guid UserId { get => _userId; set => _userId = value; }
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = (value.CompareTo(decimal.Zero) > 0) ? value : _sum;
            }
        }
        public Category Category { get => _category; set => _category = value; }
        public DateTime DateTime { get => _date; set => _date = value; }
        public string Description { get => _description; set => _description = value; }
        public Enums.Currency CurrencyOfTransaction { get => _currency; set => _currency = value; }
        public string FilePath { get => _filePath; set => _filePath = value; }

        public Transaction(Guid userId)
        {
            //  InstanceCount += 1;
            //  _id = InstanceCount;
            _guid = Guid.NewGuid();
            _userId = userId;
        }
        public Transaction(decimal sum, Enums.Currency currency, Category category, DateTime dateTime, string description, string file, Guid userId)
        {
            _sum = sum;
            _currency = currency;
            _category = category;
            _date = dateTime;
            _description = description;
            _filePath = file;
            _userId = userId;
        }

        public override bool Validate()
        {
            return (Guid != Guid.Empty) && _category != null && _date != default(DateTime) && _description != null && (UserId != Guid.Empty);
        }

    }
}
