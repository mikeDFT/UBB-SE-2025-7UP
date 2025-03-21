using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class BankAccountCreateView : Window
    {
        private BankAccountCreateViewModel viewModel;
        public BankAccountCreateView(int userID)
        {
            this.InitializeComponent();
            viewModel = new BankAccountCreateViewModel(userID);
            MainGrid.DataContext = viewModel;

            viewModel.onClose = () => this.Close();
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                viewModel.SelectedItem = new CurrencyItem { Name = radioButton.Content.ToString(), IsChecked = true };
            }
        }
    }
}
