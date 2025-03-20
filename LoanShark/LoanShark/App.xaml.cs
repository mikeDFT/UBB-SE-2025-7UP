using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using LoanShark.Domain;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        // Collection to track all active windows
        private List<Window> activeWindows = new List<Window>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            DataLink.Instance.OpenConnection();
        }

        public static void CleanupResources()
        {
            DataLink.Instance.CloseConnection();
            UserSession.Instance.InvalidateUserSession();
            Debug.Print("Resources cleaned up successfully");
        }

        // Method to register windows with the tracking system
        public void RegisterWindow(Window window)
        {
            this.activeWindows.Add(window);
            window.Closed += Window_Closed;
            Debug.Print($"Window registered. Active windows: {this.activeWindows.Count}");
        }

        // Window closed event handler
        private void Window_Closed(object sender, WindowEventArgs args)
        {
            if (sender is Window window)
            {
                this.activeWindows.Remove(window);
                Debug.Print($"Window closed. Remaining windows: {this.activeWindows.Count}");
                
                // If this was the last window, clean up resources
                if (this.activeWindows.Count == 0)
                {
                    Debug.Print("Last window closed, cleaning up resources...");
                    CleanupResources();
                }
            }
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_loginWindow = new LoginWindow();
            
            // Register this window with our tracking system
            RegisterWindow(m_loginWindow);
            
            m_loginWindow.LoginSuccess += LoginWindow_OnLoginSuccess;
            m_loginWindow.Activate();
        }

        private void LoginWindow_OnLoginSuccess(object? sender, EventArgs e)
        {
            m_mainPageWindow = new MainPageWindow();
            
            // Register the main window with our tracking system
            RegisterWindow(m_mainPageWindow);
            
            m_mainPageWindow.Activate();
            
            // Close the login window after successful login
            m_loginWindow?.Close();
        }

        private LoginWindow? m_loginWindow;
        private MainPageWindow? m_mainPageWindow;
    }
}
