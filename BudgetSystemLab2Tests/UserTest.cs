using System;
using BudgetSystemLab2Tests;
using Xunit;


namespace BudgetTests
{
    public class UserTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            var user = new User() { FirstName = "Yuliia", LastName = "Kryvosheienko", Email = "Kryvosheienko@gmail.com", Login="YuliaK" };

            //Act
            var actual = user.Validate();

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateNoName()
        {
            //Arrange
            var user = new User() { Email = "Kryvosheienko@gmail.com" };

            //Act
            var actual = user.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateNoEmail()
        {
            //Arrange
            var user = new User() { FirstName = "Yuliia", LastName = "Kryvosheienko" };

            //Act
            var actual = user.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateEmptyUser()
        {
            //Arrange
            var user = new User();

            //Act
            var actual = user.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void UserCounterTest()
        {
            //Arrange
            var user = new User();
            var user1 = new User();
            var user2 = new User();

            //Act            

            //Assert
            Assert.NotEqual(user1.Guid, user.Guid);
            Assert.NotEqual(user2.Guid, user1.Guid);
        }
        [Fact]
        public void FullNameTestValid()
        {
            //Arrange
            var user = new User { FirstName = "Yulia", LastName = "Kryvosheienko" };

            var expected = "Kryvosheienko, Yulia";

            //Act
            var actual = user.FullName;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FullNameNoFirstNameTest()
        {
            //Arrange
            var user = new User { LastName = "Kryvosheienko" };

            var expected = "Kryvosheienko";

            //Act
            var actual = user.FullName;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FullNameNoLastNameTest()
        {
            //Arrange
            var user = new User { FirstName = "Yulia" };

            var expected = "Yulia";

            //Act
            var actual = user.FullName;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}

