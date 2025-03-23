using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.View;
using Windows.System.Profile;

namespace LoanShark.Helper
{
    public static class WindowManager
    {
        // Track all active windows
        private static List<Window> activeWindows = new List<Window>();
        public static bool shouldReloadBankAccounts = false;

        // Static method to register windows with the tracking system
        public static void RegisterWindow(Window window)
        {
            activeWindows.Add(window);
            window.Closed += Window_Closed;
            Debug.Print($"Window registered. Active windows: {activeWindows.Count}");
        }

        // Static window closed event handler
        private static async void Window_Closed(object sender, WindowEventArgs args)
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
                    return;
                }

                // after closing any window, if the only window is the main page window, refresh the bank accounts flip view
                if (activeWindows.Count == 1 && activeWindows.First() is MainPageView mainWindow && shouldReloadBankAccounts)
                {
                    await RefreshBankAccounts(mainWindow);
                    shouldReloadBankAccounts = false;
                }
            }
        }

        // Static method to clean up application resources
        public static void CleanupResources()
        {
            DataLink.Instance.CloseConnection();
            UserSession.Instance.InvalidateUserSession();
            Debug.Print("Resources cleaned up successfully");
            Application.Current.Exit();
        }

        public static async Task RefreshBankAccounts(MainPageView mp_window) 
        {
            await mp_window.RefreshBankAccounts();
        }
    }
}
