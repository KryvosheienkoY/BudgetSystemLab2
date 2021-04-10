using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public class User : EntityBase
    {
        private Guid _guid;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _login;
        private List<Wallet> _wallets;
        private List<Category> _categories;



        public List<Category> Categories { get => _categories.ToList(); set => _categories = value; }
        public List<Wallet> Wallets { get => _wallets.ToList(); set => _wallets = value; }
        public string Email { get => _email; set => _email = value; }
        public string Login { get => _login; set => _login = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public Guid Guid { get => _guid; private set => _guid = value; }

        public User()
        {
            _guid = Guid.NewGuid();
            _categories = new List<Category>() {  new Category(Guid)
            {
                Name = "Default",
                Description = "",
                Color = Enums.Colors.Blue,
                CategoryIconPath = "src/iconD.png",
            }

        };
            _wallets = new List<Wallet>();
        }

        public User(Guid guid, string lastName, string firstName, string email, string login)
        {
            Guid = guid;
            _lastName = lastName;
            _firstName = firstName;
            _email = email;
            _login = login;
            _categories = new List<Category>() {  new Category(Guid)
            {
                Name = "Default",
                Description = "",
                Color = Enums.Colors.Blue,
               CategoryIconPath = "src/icon2.png",
            }

        };
            _wallets = new List<Wallet>();

        }


        public void ShareWallet(Wallet wallet, User userToShare)
        {
            wallet.AddUserToShare(Guid, userToShare);
        }

        public void AddWallet(string name, Enums.Currency currency, List<Category> categories)
        {
            Wallet myWallet = new Wallet()
            {
                Owner = Guid,
                Name = name,
                Currency = currency,
                Categories = categories.ToList(),
            };
            _wallets.Add(myWallet);
        }
        public Wallet GetWallet(Guid walletId)
        {
            foreach (Wallet w in _wallets)
            {
                if (w.Guid == walletId)
                    return w;
            }
            throw new RecordNotFoundException("No wallet with such id was found");
        }

        public void AddCategory(string name, string description, Enums.Colors color, string icon)
        {
            Category category = new Category(Guid)
            {
                Name = name,
                Description = description,
                Color = color,
                CategoryIconPath = icon,
            };
            _categories.Add(category);
        }

        public void EditCategory(Category oldCategory, string name, string description, Enums.Colors color, string icon)
        {
            //check if Category exists
            foreach (Category ct in _categories)
            {
                if (ct.Guid == oldCategory.Guid)
                {
                    ct.CategoryIconPath = icon;
                    ct.Name = name;
                    ct.Description = description;
                    ct.Color = color;
                }
            }
        }


        public void DeleteCategory(Category category)
        {
            //check if category exists
            foreach (Category ct in _categories)
            {
                if (ct.Guid == category.Guid)
                {
                    //check if wallets have this category
                    foreach (Wallet wallet in Wallets)
                    {
                        if (wallet.Categories.Contains(ct))
                            throw new DeleteException("Category couldn`t be deleted, because yor wallet is using it.");
                    }
                    _categories.Remove(ct);
                    return;
                }
                throw new DeleteException("Category couldn`t be deleted, because it doesn`t exist.");
            }
        }

        public override bool Validate()
        {
            if (!(Guid == Guid.Empty) || String.IsNullOrWhiteSpace(LastName) || String.IsNullOrWhiteSpace(Email))
                return false;
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
            Match match = regex.Match(Email);
            if (match.Success)
                return true;
            return false;
        }

        public string FullName
        {
            get
            {
                var result = LastName;
                if (!String.IsNullOrWhiteSpace(FirstName))
                {
                    if (!String.IsNullOrWhiteSpace(LastName))
                    {
                        result += ", ";
                    }
                    result += FirstName;
                }
                return result;
            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
