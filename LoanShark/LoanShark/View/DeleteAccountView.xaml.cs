using System.Threading.Tasks;
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LoanShark.ViewModel;
using LoanShark.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteAccountView : Window
    {
        private readonly DeleteAccountViewModel viewModel;

        public DeleteAccountView()
        {
            this.InitializeComponent();
            viewModel = new DeleteAccountViewModel();
            MainGrid.DataContext = viewModel;
            WindowManager.RegisterWindow(this);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInput.Password))
            {
                await ShowErrorDialog("Please enter your password.");
                return;
            }

            var result = await viewModel.DeleteAccount(PasswordInput.Password);

            if (result == "Succes")
            {
                // automatically invalidates the session in constructor
                WindowManager.LogOut();
            }
            else
            {
                await ShowErrorDialog("Failed to delete account: " + result);
            }
        }

        private async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
