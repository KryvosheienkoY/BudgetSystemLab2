﻿using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2.Entities
{

    public class DBWallet : EntityBase, IStorable
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Owner { get; set; }
        public Guid Guid { get; }

        public List<DBTransaction> _transactions;
        public List<DBTransaction> Transactions
        {
            get
            {
                List<DBTransaction> transactionsList = new List<DBTransaction>();
                foreach (DBTransaction transaction in _transactions)
                {
                    transactionsList.Add(transaction);
                }
                return transactionsList;
            }
            set => _transactions = value;
        }
       

      
        public DBWallet(Guid guid, string name, decimal balance, string currency, string owner )
        {
            Guid = guid;
            Currency = currency;
            Name = name;
            Balance = balance;
            Owner = owner;
            _transactions = new List<DBTransaction>();
        }
        public override string ToString()
        {
            return $"{Name} ({Balance})";
        }
        public void AddTransaction(Guid userId, DBTransaction transaction)
        {
            //if (!(Owner == userId || IfUserHasAccess(userId)))
            //    throw new NoAccessException("You have no access to add this transaction.");
            if (!transaction.IsValid)
                throw new ValidationException("DBTransaction is invalid.");

            Balance += CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, Currency);
            _transactions.Add(transaction);

        }
        public void EditTransaction(Guid userId, DBTransaction oldTransaction, decimal sum, string currency, Category category, DateTime dateTime, string description, string file)
        {
            //if (Owner != userId)
            //    throw new NoAccessException("You have no access to edit this transaction.");
            //check if transaction exists
            foreach (DBTransaction tr in _transactions)
            {
                if (tr.Guid == oldTransaction.Guid)
                {
                    Balance += CurrencyConverter.Convert(sum, currency, Currency) - CurrencyConverter.Convert(oldTransaction.Sum, oldTransaction.CurrencyOfTransaction, Currency);
                    tr.Sum = sum;
                    tr.CurrencyOfTransaction = currency;
                    //tr.Category = category;
                    tr.DateTime = dateTime;
                    tr.Description = description;
                    return;
                }
            }
            throw new RecordNotFoundException("DBTransaction wasn`t found.");
        }
        public void DeleteTransaction(Guid userId, DBTransaction transaction)
        {
            //check if transaction exists
            foreach (DBTransaction tr in _transactions)
            {
                if (tr.Guid == transaction.Guid)
                {
                    _transactions.Remove(tr);
                    Balance -= CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, Currency);
                    return;
                }
            }
            throw new RecordNotFoundException("DBTransaction wasn`t found.");
        }

        public decimal GetLastMonthEarnings()
        {
            List<DBTransaction> lastMonthTransactionsList = GetLastMonthTransactions();
            decimal earnings = decimal.Zero;
            foreach (DBTransaction tr in lastMonthTransactionsList)
            {
                if (tr.Sum > 0)
                {
                    earnings += CurrencyConverter.Convert(tr.Sum, tr.CurrencyOfTransaction, Currency);
                }
            }
            return earnings;
        }
        public decimal GetLastMonthSpendings()
        {
            List<DBTransaction> lastMonthTransactionsList = GetLastMonthTransactions();
            decimal spendings = decimal.Zero;
            foreach (DBTransaction tr in lastMonthTransactionsList)
            {
                if (tr.Sum < 0)
                {
                    spendings += CurrencyConverter.Convert(tr.Sum, tr.CurrencyOfTransaction, Currency);
                }
            }
            return spendings;
        }

        public List<DBTransaction> GetTransactions(int idFrom, int idTo)
        {
            List<DBTransaction> transactionsList = new List<DBTransaction>();
            for (int i = 0; i < _transactions.Count; i++)
            {
                if (i >= idFrom && i <= idTo)
                    transactionsList.Add(_transactions[i]);
                // no more than 10 transactions
                if (transactionsList.Count == 10)
                    break;
            }
            return transactionsList;
        }


        private List<DBTransaction> GetLastMonthTransactions()
        {
            List<DBTransaction> lastMonthTransactionsList = new List<DBTransaction>();
            foreach (DBTransaction tr in Transactions)
            {
                if (tr.DateTime.Month.Equals(DateTime.Now.AddMonths(-1).Month))
                {
                    lastMonthTransactionsList.Add(tr);
                }
            }
            return lastMonthTransactionsList;
        }
        public override bool Validate()
        {
            return (Guid != Guid.Empty) && !String.IsNullOrWhiteSpace(Owner) && !String.IsNullOrWhiteSpace(Name);
        }
    }

}



