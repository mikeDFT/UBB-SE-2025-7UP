using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using LoanShark.ViewModel;
using System.Threading.Tasks;
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
        private readonly DeleteAccountViewModel _viewModel;

        public DeleteAccountView()
        {
            this.InitializeComponent();
            _viewModel = new DeleteAccountViewModel();
            MainGrid.DataContext = _viewModel;
            WindowManager.RegisterWindow(this);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInput.Password))
            {
                await ShowErrorDialog("Please enter your password.");
                return;
            }

            var result = await _viewModel.DeleteAccount(PasswordInput.Password);
            
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
