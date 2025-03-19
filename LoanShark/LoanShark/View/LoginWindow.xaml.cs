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
using System.Diagnostics;
using LoanShark.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        public LoginViewModel viewModel { get; private set; }
        
        public LoginWindow()
        {
            this.InitializeComponent();
            this.viewModel = new LoginViewModel();
        }

        public void LoginButtonHandler(object sender, RoutedEventArgs e) 
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.viewModel.ErrorMessage = "Email and password cannot be empty";
                this.viewModel.IsErrorVisible = true;
                return;
            }

            if (this.viewModel.ValidateCredentials(email, password))
            {
                // navigate to the main window
                Debug.Print("Login successful");
                // Close the current login window
                // this.Close();
                
                // Create and activate the main window
                // var mainWindow = new MainWindow();
                // mainWindow.Activate();
            }
            else
            {
                // The ViewModel.IsErrorVisible and ViewModel.ErrorMessage will be updated by the ValidateCredentials method
                // and the UI will be updated automatically through binding
                passwordBox.Password = string.Empty;
                passwordBox.Focus(FocusState.Programmatic);
            }
        }

        private void EmailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                passwordBox.Focus(FocusState.Programmatic);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                LoginButtonHandler(sender, e);
            }
        }
    }
}
