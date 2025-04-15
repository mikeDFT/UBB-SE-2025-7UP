using System;
using System.Threading.Tasks;
using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LoanShark.Helper;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountVerifyView : Window
    {
        private BankAccountVerifyViewModel viewModel;
        public BankAccountVerifyView()
        {
            this.InitializeComponent();
            viewModel = new BankAccountVerifyViewModel();
            MainGrid.DataContext = viewModel;

            viewModel.OnSuccess = async () => await ShowSuccessMessage();
            viewModel.OnClose = () => this.Close();
            viewModel.OnFailure = async () => await ShowFailureMessage();

            WindowManager.RegisterWindow(this);
        }

        private async Task ShowSuccessMessage()
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = "Bank account deleted!",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync().AsTask();

            viewModel.OnClose?.Invoke();
            // update main page view model
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                viewModel.OnConfirmButtonClicked();
            }
        }

        private async Task ShowFailureMessage()
        {
            var dialog = new ContentDialog
            {
                Title = "Failure",
                Content = "Wrong credentials. Try again.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync().AsTask();
        }
    }
}
