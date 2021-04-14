using BudgetSystemLab2;
using BudgetSystemLab2.Services;
using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BudgetTests
{
    public class Authentication
    {
        [Fact]
        public void checkRegistrationAsync()
        {
            string login = RandomDigits(20);
            string pswd = RandomDigits(20);
            string lastName = RandomDigits(20);
            string firstName = RandomDigits(20);
            string email = "test@t.com";
            RegistrationUser regUser = new RegistrationUser() { Login=login, Password=pswd, LastName=lastName,FirstName=firstName,Email=email};

            AuthenticationService _service = new AuthenticationService();
            Assert.True(_service.RegisterUserAsync(regUser).Result);
            Assert.ThrowsAsync<Exception>(() => _service.RegisterUserAsync(regUser));
        }
        [Fact]
        public async Task CheckLoginAsync()
        {
            string login = RandomDigits(20);
            string pswd = RandomDigits(20);
            string lastName = RandomDigits(20);
            string firstName = RandomDigits(20);
            string email = "test@trr.com";
            RegistrationUser regUser = new RegistrationUser() { Login=login, Password=pswd, LastName=lastName,FirstName=firstName,Email=email};
            AuthenticationUser authUser = new AuthenticationUser() { Login=login, Password=pswd};
           
            AuthenticationService _service = new AuthenticationService();
            //try to login unregisted user
            _ = Assert.ThrowsAsync<Exception>(() => _service.AuthenticateAsync(authUser));
            _ =  await _service.RegisterUserAsync(regUser);
            var user = await _service.AuthenticateAsync(authUser);
            Assert.Equal(login,user.Login);
        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
