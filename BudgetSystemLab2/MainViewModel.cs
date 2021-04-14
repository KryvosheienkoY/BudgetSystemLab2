

using BudgetSystemLab2.Authentication;
using BudgetSystemLab2.Navigation;
using BudgetSystemLab2.Wallets;

namespace BudgetSystemLab2
{
    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {
        public MainViewModel()
        {
            Navigate(MainNavigatableTypes.Auth);
        }

        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            if (type == MainNavigatableTypes.Auth)
            {
                return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets));
            }
            else
            {
                //return new WalletBaseViewModel();
                return new WalletsViewModel();
            }
        }
    }
}
