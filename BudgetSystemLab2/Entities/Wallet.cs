using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public class Wallet : EntityBase, IStorable
    {
        private static int InstanceCount;
        private Guid _guid;
        private Guid _ownerId;
        private decimal _balance;
        private string _name;
        private Enums.Currency _currency;
        public List<Category> _categories;
        public List<Transaction> _transactions;
        public List<User> _sharedUsers;

        public Wallet()
        {
           // InstanceCount += 1;
            //_id = InstanceCount;
           _transactions = new List<Transaction>();
            _sharedUsers = new List<User>();
            _guid = Guid.NewGuid();
            var defaultCategory = new Category(_ownerId)
            {
                Name = "default",
                Description = " ",
                Color = Enums.Colors.Red,
                CategoryIconPath = "src/icon1.png",
            };
            _categories = new List<Category>() { defaultCategory };
        }
        public Wallet(Guid guid, Guid owner, string name, Enums.Currency currency, List<Category> categories)
        {
            _guid = guid;
            _ownerId = owner;
            _name = name;
            _currency = currency;
            _categories = categories.ToList();
            _balance = decimal.Zero;
            _transactions = new List<Transaction>();
            _sharedUsers = new List<User>();
        }
        private bool IfUserHasAccess(Guid userId)
        {
            foreach (User user in _sharedUsers)
            {
                if (user.Guid == userId)
                    return true;
            }
            return false;
        }

        public void AddTransaction(Guid userId, Transaction transaction)
        {
            if (!(Owner == userId || IfUserHasAccess(userId)))
                throw new NoAccessException("You have no access to add this transaction.");
            if (!transaction.IsValid)
                throw new ValidationException("Transaction is invalid.");

            Balance += CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, Currency);
            _transactions.Add(transaction);

        }
        public void EditTransaction(Guid userId, Transaction oldTransaction, decimal sum, Enums.Currency currency, Category category, DateTime dateTime, string description, string file)
        {
            if (Owner != userId)
                throw new NoAccessException("You have no access to edit this transaction.");
            //check if transaction exists
            foreach (Transaction tr in _transactions)
            {
                if (tr.Guid == oldTransaction.Guid)
                {
                    Balance += CurrencyConverter.Convert(sum, currency, Currency) - CurrencyConverter.Convert(oldTransaction.Sum, oldTransaction.CurrencyOfTransaction, Currency);
                    tr.Sum = sum;
                    tr.CurrencyOfTransaction = currency;
                    tr.Category = category;
                    tr.DateTime = dateTime;
                    tr.Description = description;
                    tr.FilePath = file;
                    return;
                }
            }
            throw new RecordNotFoundException("Transaction wasn`t found.");
        }
        public void DeleteTransaction(Guid userId, Transaction transaction)
        {
            //check if transaction exists
            foreach (Transaction tr in _transactions)
            {
                if (tr.Guid == transaction.Guid)
                {
                    _transactions.Remove(tr);
                    Balance -= CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, Currency);
                    return;
                }
            }
            throw new RecordNotFoundException("Transaction wasn`t found.");
        }

        public decimal GetLastMonthEarnings()
        {
            List<Transaction> lastMonthTransactionsList = GetLastMonthTransactions();
            decimal earnings = decimal.Zero;
            foreach (Transaction tr in lastMonthTransactionsList)
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
            List<Transaction> lastMonthTransactionsList = GetLastMonthTransactions();
            decimal spendings = decimal.Zero;
            foreach (Transaction tr in lastMonthTransactionsList)
            {
                if (tr.Sum < 0)
                {
                    spendings += CurrencyConverter.Convert(tr.Sum, tr.CurrencyOfTransaction, Currency);
                }
            }
            return spendings;
        }

        public List<Transaction> GetTransactions(int idFrom, int idTo)
        {
            List<Transaction> transactionsList = new List<Transaction>();
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


        private List<Transaction> GetLastMonthTransactions()
        {
            List<Transaction> lastMonthTransactionsList = new List<Transaction>();
            foreach (Transaction tr in Transactions)
            {
                if (tr.DateTime.Month.Equals(DateTime.Now.AddMonths(-1).Month))
                {
                    lastMonthTransactionsList.Add(tr);
                }
            }
            return lastMonthTransactionsList;
        }


        public Guid Guid { get => _guid; private set => _guid = value; }
        public decimal Balance { get => _balance;  set => _balance = value; }
        public string Name { get => _name; set => _name = value; }
        public Guid Owner { get => _ownerId; set => _ownerId = value; }
        public Enums.Currency Currency { get => _currency; set => _currency = value; }
        public List<Category> Categories
        {
            get
            {
                List<Category> categoriesList = new List<Category>();
                foreach (Category category in _categories)
                {
                    categoriesList.Add(category);
                }
                return categoriesList;
            }
            set
            {
                if (value.Count > 0)
                    _categories = value;
            }
        }
        public List<Transaction> Transactions
        {
            get
            {
                List<Transaction> transactionsList = new List<Transaction>();
                foreach (Transaction transaction in _transactions)
                {
                    transactionsList.Add(transaction);
                }
                return transactionsList;
            }
            set => _transactions = value;
        }

        public void AddUserToShare(Guid ownerId, User userToShare)
        {
            //check if user is wallets owner
            if (Owner == ownerId)
            {
                _sharedUsers.Add(userToShare);
            }
        }

        public override bool Validate()
        {
            return (Guid != Guid.Empty) && Categories.Count > 0 && (Owner != Guid.Empty) && !String.IsNullOrWhiteSpace(Name);
        }
    }
}
