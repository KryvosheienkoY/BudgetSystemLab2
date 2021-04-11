using System;
using System.Collections.Generic;
using System.Drawing;
using BudgetSystemLab2;
using Xunit;
namespace BudgetTests
{
    public class WalletTest
    {
        [Fact]
        public void ValidateWalletMethods()
        {
            ValidateValid();
            checkTransactionMethods();

        }
        [Fact]
        public void ValidateValid()
        {

            //creating user
            var user = new User() { FirstName = "Yuliia", LastName = "Kryvosheienko", Email = "Kryvosheienko@gmail.com", Login="YK" };
            user.AddCategory("Cinema", "Cinema spendings", Enums.Colors.Red, "src/icon1.png");
            user.AddCategory("Food", "Food spendings", Enums.Colors.Pink, "src/icon2.png");
            //adding wallet
            user.AddWallet("myWallet", Enums.Currency.USD, user.Categories);
            var ifValidWallet = true;
            foreach (Wallet w in user.Wallets)
                ifValidWallet = ifValidWallet && w.Validate();
            Assert.True(ifValidWallet);
        }

        private void checkTransactionMethods()
        {
            //creating user
            var user = new User() { FirstName = "User", LastName = "Kryvosheienko", Email = "Kryvosheienko@gmail.com", Login = "YK" };
            user.AddCategory("Cinema", "Cinema spendings", Enums.Colors.Red, "src/icon1.png");
            user.AddCategory("Food", "Food spendings", Enums.Colors.Pink, "src/icon2.png");
            //adding wallet
            user.AddWallet("mySWallet", Enums.Currency.USD, user.Categories);
            Wallet userWallet1 = user.Wallets[0];
            //check that wallet has no transactions
            Assert.True(userWallet1.Transactions.Count == 0);

            //create transaction
            Category category1 = userWallet1.Categories[0];
            var transaction = new Transaction(user.Guid)
            {
                Sum = -40m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.UAH,
                Category = category1,
                DateTime = DateTime.Now,
            };

            try
            {
                userWallet1.AddTransaction(userWallet1.Owner, transaction);
            }
            catch (Exception e)
            {
            }
            //check that wallet has 1 transaction
            Assert.Single(userWallet1.Transactions);

            //check that balance changed correctly
            Assert.True(userWallet1.Balance == CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, userWallet1.Currency));
            //edit transaction
            transaction.Sum = -45m;
            // decimal editedSum = -45m;
            userWallet1.EditTransaction(userWallet1.Owner, transaction, transaction.Sum, transaction.CurrencyOfTransaction, transaction.Category, transaction.DateTime, transaction.Description, transaction.FilePath);
            //check that balance changed correctly
            Assert.Equal(userWallet1.Balance, CurrencyConverter.Convert(transaction.Sum, transaction.CurrencyOfTransaction, userWallet1.Currency));
            //let`s delete transaction and check balance
            userWallet1.DeleteTransaction(userWallet1.Owner, transaction);
            Assert.Equal(userWallet1.Balance, decimal.Zero);

            //create more transactions
            var transaction1 = new Transaction(user.Guid)
            {
                Sum = +400m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now,
              
            };
            var transaction2 = new Transaction(user.Guid)
            {
                Sum = +500m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.UAH,
                Category = category1,
                DateTime = DateTime.Now,
            };
            var transaction3 = new Transaction(user.Guid)
            {
                Sum = +900m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now,
            };
            var transaction4 = new Transaction(user.Guid)
            {
                Sum = -1000m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction5 = new Transaction(user.Guid)
            {
                Sum = +400m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.USD,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction6 = new Transaction(user.Guid)
            {
                Sum = +480m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction7 = new Transaction(user.Guid)
            {
                Sum = -10m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction8 = new Transaction(user.Guid)
            {
                Sum = -30m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction9 = new Transaction(user.Guid)
            {
                Sum = +120m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            var transaction10 = new Transaction(user.Guid)
            {
                Sum = -150m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.EUR,
                Category = category1,
                DateTime = DateTime.Now.AddMonths(-1),
            };
            List<Transaction> myTransactions = new List<Transaction>()
            {
            transaction1,
            transaction2,
            transaction3,
            transaction4,
            transaction5,
            transaction6,
            transaction7,
            transaction8,
            transaction9,
            transaction10,
            };
            foreach (Transaction tr in myTransactions)
                userWallet1.AddTransaction(userWallet1.Owner, tr);


            //check GetLastMonthEarnings and GetLastMonthSpendings methods

            decimal earnings = decimal.Zero;
            decimal spendings = decimal.Zero;
            foreach (Transaction tr in myTransactions)
            {
                if (tr.DateTime.Month.Equals(DateTime.Now.AddMonths(-1).Month))
                {
                    decimal amount = CurrencyConverter.Convert(tr.Sum, tr.CurrencyOfTransaction, userWallet1.Currency);
                    if (tr.Sum > 0)
                        earnings += amount;
                    else
                        spendings += amount;
                }
            }
            Assert.Equal(earnings, userWallet1.GetLastMonthEarnings());
            Assert.Equal(spendings, userWallet1.GetLastMonthSpendings());

            //test getTransactions()
            List<Transaction> list = userWallet1.GetTransactions(0, 11);
            Assert.Equal(10, list.Count);

            // let`s share wallet with user2
            var user2 = new User() { FirstName = "Tolyan", LastName = "Tolyanenko", Email = "t.tolyan@gmail.com", Login="TolyaTolya" };
            userWallet1.AddUserToShare(user.Guid, user2);
            //try to add transaction
            try
            {
                userWallet1.AddTransaction(user2.Guid, transaction);
            }
            catch (Exception e)
            {

            }
        }
    }
}
