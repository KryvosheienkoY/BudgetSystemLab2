using System.Windows.Controls;

namespace BudgetSystemLab2.Wallets
{
    /// <summary>
    /// Interaction logic for WalletDetails.xaml
    /// </summary>
    public partial class WalletDetailsView : UserControl
    {
        public WalletDetailsView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //private void Currency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var st = currencySelect.SelectedItem.ToString();
        //}

    }
}
