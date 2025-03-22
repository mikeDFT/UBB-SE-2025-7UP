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
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using LoanShark.Domain;
using LoanShark.Helper;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// Window for updating bank account details.
    /// </summary>
    public sealed partial class BankAccountUpdateView : Window
    {
        private BankAccountUpdateViewModel? _viewModel;

        // Initializes the view and makes the window size be 800*1400
        public BankAccountUpdateView()
        {
            try
            {
                
                this.InitializeComponent();

                // Initialize the ViewModel after the component is initialized
                _viewModel = new BankAccountUpdateViewModel();

                AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 1400));
                MainGrid.DataContext = _viewModel;

                _viewModel.OnUpdateSuccess = () => 
                {
                    this.Close();
                };
                
                _viewModel.OnClose = () => 
                {
                    this.Close();
                };

                WindowManager.RegisterWindow(this);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing BankAccountUpdateView: {ex.Message}");
                ShowErrorDialog("Initialization Error", $"Failed to initialize the bank account update view: {ex.Message}");
            }
        }

        // handles the Update Button Click
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel == null)
                {
                    await ShowDialog("Error", "ViewModel not initialized properly.", "OK");
                    return;
                }
                
                string result = await _viewModel.UpdateBankAccount();
                
                if (result == "Success")
                {
                    await ShowDialog("Success", "Bank account updated successfully.", "OK");
                    _viewModel.OnUpdateSuccess?.Invoke();
                }
                else
                {
                    await ShowDialog("Error", result, "OK");
                }
            }
            catch (Exception ex)
            {
                await ShowDialog("Error", $"Failed to update bank account: {ex.Message}", "OK");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.DeleteBankAccount();
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Error", $"Failed to delete bank account: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async System.Threading.Tasks.Task ShowDialog(string title, string content, string closeButtonText)
        {
            try
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = title,
                    Content = content,
                    CloseButtonText = closeButtonText,
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing dialog: {ex.Message}");
            }
        }
        
        private void ShowErrorDialog(string title, string message)
        {
            // Create a separate method to show error dialogs outside the normal UI thread
            DispatcherQueue.TryEnqueue(() => {
                var errorDialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK"
                };
                
                if (Content != null && Content.XamlRoot != null)
                {
                    errorDialog.XamlRoot = Content.XamlRoot;
                    _ = errorDialog.ShowAsync();
                }
            });
        }
    }
}
