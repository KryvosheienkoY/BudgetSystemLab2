using BudgetSystemLab2;
using System;
using System.Drawing;
using Xunit;
namespace BudgetTests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            var transaction = new Transaction(Guid.NewGuid())
            {
                Sum = -40m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.UAH,
                Category = new Category(Guid.NewGuid())
                {
                    Name = "Cinema",
                    Description = "Cinema spendings",
                    Color = Enums.Colors.Orange,
                    CategoryIconPath = "src/icon1.png",
                },
                DateTime = DateTime.Now,
                FilePath = "/src/photo.png"
            };
            var transaction1 = new Transaction(Guid.NewGuid())
            {
                Sum = -40m,
                Description = "Cinema spendings in 40 m",
                CurrencyOfTransaction = Enums.Currency.UAH,
                Category = new Category(Guid.NewGuid())
                {
                    Name = "Cinema",
                    Description = "Cinema spendings",
                    Color = Enums.Colors.Orange,
                    CategoryIconPath = "src/icon1.png",
                },
                DateTime = DateTime.Now,
            };

            //Act
            var actual = transaction.Validate();
            var actual1 = transaction1.Validate();

            //Assert
            Assert.True(actual);
            Assert.True(actual1);
        }

    }
}
