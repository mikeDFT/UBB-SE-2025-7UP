using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using LoanShark.View;
using LoanShark.Data;
using System.Diagnostics;
using LoanShark.Helper;
using LoanShark.Domain;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    public partial class App : Application
    {
        public App()
        {
            Debug.Print("Application is now opening...");
            this.InitializeComponent();
            DataLink.Instance.OpenConnection();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_loginWindow = new LoginWindow();
            m_loginWindow.LoginSuccess += LoginWindow_OnLoginSuccess; // event handler
            m_loginWindow.Activate();
        }

        private void LoginWindow_OnLoginSuccess(object? sender, EventArgs e)
        {
            m_mainPageWindow = new MainPageWindow();
            m_mainPageWindow.LogOut += MainPageWindow_OnLogOut; // event handler
            
            // MainPageWindow registers itself in its constructor
            m_mainPageWindow.Activate();
            
            // Close the login window after successful login
            m_loginWindow?.Close();
        }
        
        private void MainPageWindow_OnLogOut(object? sender, EventArgs e)
        {
            m_loginWindow = new LoginWindow();
            m_loginWindow.LoginSuccess += LoginWindow_OnLoginSuccess; // event handler
            m_loginWindow.Activate();

            // Close the main page window after logging out
            m_mainPageWindow?.Close();
        }

        private LoginWindow? m_loginWindow;
        private MainPageWindow? m_mainPageWindow;
    }
}
