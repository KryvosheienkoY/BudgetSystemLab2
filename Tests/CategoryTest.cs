using BudgetSystemLab2;
using System;
using System.Drawing;
using Xunit;

namespace BudgetTests
{
    public class CategoryTest
    {
        [Fact]
        public void ValidateValid()
        {

            //Arrange
            var category = new Category(Guid.NewGuid())
            {
                Name = "Cinema",
                Description = "Cinema spendings",
                Color = Enums.Colors.Red,
                CategoryIconPath = "src/icon1.png",
            };

            //Act
            var actual = category.Validate();

            //Assert
            Assert.True(actual);
        }
    }
}
