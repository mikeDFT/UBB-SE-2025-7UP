using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using LoanShark.Helper;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountVerifyView : Window
    {
        BankAccountVerifyViewModel viewModel;
        public BankAccountVerifyView(string IBAN, string email)
        {
            this.InitializeComponent();
            viewModel = new BankAccountVerifyViewModel(IBAN, email);
            MainGrid.DataContext = viewModel;

            viewModel.OnSuccess = async () => await ShowSuccessMessage();
            viewModel.OnClose = () => this.Close();
            viewModel.OnFailure = async () => await ShowFailureMessage();

            CloseWindowService.CloseAllWindows += CloseWindow;
        }

        private void CloseWindow()
        {
            this.Close();
        }

        ~BankAccountVerifyView()
        {
            CloseWindowService.CloseAllWindows -= CloseWindow;
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

            CloseWindowService.RequestCloseAllWindows();
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
