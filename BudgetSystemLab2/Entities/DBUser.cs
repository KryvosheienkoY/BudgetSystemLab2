using DataStorage;
using System;
using System.Collections.Generic;

namespace BudgetSystemLab2
{
    public class DBUser : IStorable
    {
        public Guid Guid { get; }
        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get;}
        public string Login { get;}
        public string Password { get;}

        public DBUser(string firstName, string lastName, string email, string login, string password)
        {
            Guid = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
          //  GuidsOfWallets = guids;
        }

    }
}
