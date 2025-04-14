using System;
using System.Diagnostics;
using LoanShark.Domain;
using LoanShark.Helper;
using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Window
    {
        public LoginViewModel ViewModel { get; private set; }
        public event EventHandler? LoginSuccess;

        public LoginView()
        {
            this.InitializeComponent();
            this.ViewModel = new LoginViewModel();

            // Register this window with the WindowManager
            WindowManager.RegisterWindow(this);

            UserSession.Instance.InvalidateUserSession();
        }

        public async void LoginButtonHandler(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.ViewModel.ErrorMessage = "Email and password cannot be empty";
                this.ViewModel.IsErrorVisible = true;
                return;
            }

            if (await this.ViewModel.ValidateCredentials(email, password))
            {
                // navigate to the main window
                Debug.Print("Login successful");
                await this.ViewModel.InstantiateUserSessionAfterLogin(email);
                OpenMainPageWindow();
            }
            else
            {
                // The ViewModel.IsErrorVisible and ViewModel.ErrorMessage will be updated by the ValidateCredentials method
                // and the UI will be updated automatically through binding
                passwordBox.Password = string.Empty;
                passwordBox.Focus(FocusState.Programmatic);
            }
        }

        public void SignUpButtonHandler(object sender, RoutedEventArgs e)
        {
            // navigate to the sign up window
            UserRegistrationView userRegistrationWindow = new UserRegistrationView();
            userRegistrationWindow.Activate();
            this.Close();
        }

        private void OpenMainPageWindow() // opens the main page window and closes the login window
        {
            WindowManager.ShouldReloadBankAccounts = false; // bank accounts are loaded by the constructor of the main page window
            MainPageView mainPageWindow = new MainPageView();
            mainPageWindow.Activate();
            this.Close();
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
                // no await here, because the LoginButtonHandler is not async
                // it is async void, but you don't need to await it
                // Event handlers are not awaited by default
                LoginButtonHandler(sender, e);
            }
        }
    }
}
