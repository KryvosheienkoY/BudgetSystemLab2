using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BudgetSystemLab2.Utils;
using DataStorage;

namespace BudgetSystemLab2.Services
{
    public class AuthenticationService
    {
        private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();
        private PasswordEncrypter encrypter = new PasswordEncrypter(20);

        public async Task<User> AuthenticateAsync(AuthenticationUser authUser)
        {
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();
            //hash user`s pswd
            //byte[] pswd = Encoding.ASCII.GetBytes(authUser.Password);
            //byte[] salt = Encoding.ASCII.GetBytes(encrypter.Salt);
            //byte[] encryptedPswd =encrypter.GenerateSaltedHash(pswd, salt);
            //var dbUser= new DBUser("","","","","");
            //foreach (DBUser user in users)
            //{
            //    if(user.Login == authUser.Login)
            //    {
            //        if (encrypter.CompareByteArrays(encryptedPswd, Encoding.ASCII.GetBytes(user.Password)))
            //        {
            //            dbUser = user;
            //        }
            //    }
            //}
            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password==authUser.Password);
            if (dbUser.Login == "")
                throw new Exception("Wrong Login or Password");
            LoginedUser.User = dbUser;
            return new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login);
        }

        public async Task<bool> RegisterUserAsync(RegistrationUser regUser)
        {
            Thread.Sleep(2000);
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password) || String.IsNullOrWhiteSpace(regUser.LastName))
                throw new ArgumentException("Login, Password or Last Name is Empty");
            ////encrypt pswd
            //byte[] pswd = Encoding.ASCII.GetBytes(regUser.Password);
            //byte[] salt = Encoding.ASCII.GetBytes(encrypter.Salt);
            //string encryptedPswd = Encoding.ASCII.GetString(encrypter.GenerateSaltedHash(pswd, salt)); 
            //dbUser = new DBUser(regUser.LastName + regUser.FirstName, regUser.LastName, regUser.Email,
            //    regUser.Login, encryptedPswd); 
            dbUser = new DBUser(regUser.LastName + regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, regUser.Password);
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}
