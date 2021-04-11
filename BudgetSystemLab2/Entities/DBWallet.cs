using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Entities
{

    public class DBWallet : IStorable
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Owner { get; set; }
        public Guid Guid { get;}

        public override string ToString()
        {
            return $"{Name} ({Balance} {Currency})";
        }

        public DBWallet(Guid guid, string name, decimal balance, string currency, string owner )
        {
            Guid = guid;
            Currency = currency;
            Name = name;
            Balance = balance;
            Owner = owner;
        }
    }

}



