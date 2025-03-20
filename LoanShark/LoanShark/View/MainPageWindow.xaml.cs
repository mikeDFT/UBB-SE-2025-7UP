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
using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.ViewModel;
using LoanShark.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageWindow : Window
    {
        public event EventHandler? LogOut;

        public MainPageViewModel ViewModel { get; private set; }
        
        public MainPageWindow()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            
            // Register this window with the WindowManager
            WindowManager.RegisterWindow(this);
            
            // Set the welcome text from ViewModel
            centeredTextField.Text = this.ViewModel.WelcomeText;
        }

        public void CheckBalanceButtonHandler(object sender, RoutedEventArgs e)
        {
            // TODO: Implement balance check
        }
        
        private void AccountsFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipView = sender as FlipView;
            if (flipView?.SelectedItem is BankAccount selectedAccount)
            {
                // Optional: You can handle the selection change here
                Debug.Print($"Selected account: {selectedAccount.CustomName}, IBAN: {selectedAccount.IBAN}");
                
                try
                {
                    UserSession.Instance.SetUserData("current_bank_account_iban", selectedAccount.IBAN);
                }
                catch (Exception ex)
                {
                    Debug.Print($"Error setting current bank account: {ex.Message}");
                }
            }
        }

        private void AccountSettingsButtonHandler(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void LogOutButtonHandler(object sender, RoutedEventArgs e)
        {
                // First, invoke the LogOut event
                this.LogOut?.Invoke(this, EventArgs.Empty);

                // Only after all handlers have completed, invalidate the session
                UserSession.Instance.InvalidateUserSession();
        }

        private void ExitLoanSharkButtonHandler(object sender, RoutedEventArgs e)
        {
            WindowManager.CleanupResources();
        }
    }
}
