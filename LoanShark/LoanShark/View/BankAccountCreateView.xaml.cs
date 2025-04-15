using System.Threading.Tasks;
using System;
using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LoanShark.Helper;

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

        public BankAccountCreateView()
        {
            this.InitializeComponent();
            viewModel = new BankAccountCreateViewModel();
            MainGrid.DataContext = viewModel;

            viewModel.OnClose = () => this.Close();
            viewModel.OnSuccess = async () => await this.ShowSuccessMessage();

            WindowManager.RegisterWindow(this);
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                viewModel.SelectedItem = new CurrencyItem { Name = radioButton.Content.ToString() ?? string.Empty, IsChecked = true };
            }
        }

        private async Task ShowSuccessMessage()
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = "Bank account creation was successful!",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync().AsTask();

            this.Close();
        }
    }
}
