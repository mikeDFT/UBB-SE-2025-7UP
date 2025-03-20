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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageWindow : Window
    {
        // Static collection to track all active windows
        private static List<Window> activeWindows = new List<Window>();
        
        public MainPageWindow()
        {
            this.InitializeComponent();
            // Register this window
            RegisterWindow(this);
            
            // Set the welcome text from UserSession
            try {
                string? firstName = UserSession.Instance.GetUserData("first_name");
                centeredTextField.Text = firstName != null ? $"Welcome back, {firstName}" : "Welcome, user";
            }
            catch (Exception ex) {
                centeredTextField.Text = "Welcome, user";
                Debug.Print($"Error getting user data: {ex.Message}");
            }
        }

        // Static method to register windows with the tracking system
        public static void RegisterWindow(Window window)
        {
            activeWindows.Add(window);
            window.Closed += Window_Closed;
            Debug.Print($"Window registered. Active windows: {MainPageWindow.activeWindows.Count}");
        }

        // Static window closed event handler
        private static void Window_Closed(object sender, WindowEventArgs args)
        {
            if (sender is Window window)
            {
                activeWindows.Remove(window);
                Debug.Print($"Window closed. Remaining windows: {activeWindows.Count}");
                
                // If this was the last window, clean up resources
                if (activeWindows.Count == 0)
                {
                    Debug.Print("Last window closed, cleaning up resources...");
                    CleanupResources();
                }
            }
        }

        // Static method to clean up application resources
        public static void CleanupResources()
        {
            DataLink.Instance.CloseConnection();
            UserSession.Instance.InvalidateUserSession();
            Debug.Print("Resources cleaned up successfully");
        }

        private void AccountSettingsButtonHandler(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void LogOutButtonHandler(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void ExitLoanSharkButtonHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
