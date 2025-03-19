using LoanShark.Domain;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LoanShark.ViewModel;
using System;
using System.Diagnostics;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;

namespace LoanShark
{
    public partial class LoanView : Window
    {
        public LoanView(int userID)
        {
            InitializeComponent();
            var ViewModel = new LoanViewModel(userID);
            MainGrid.DataContext = ViewModel;

            ViewModel.CloseAction = () => this.Close(); // the closing action

            // Resizing window to see the whole table
            resizeWindow(1800, 900);
        }

        private void resizeWindow(int width, int height)
        {
            // Set window size after initialization
            var windowHandle = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Set window size
            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        }

        // Making sure if the user erases the text, the amount becomes 0
        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // This method is needed to handle empty text values
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    // If text is empty, set Amount to 0 in the ViewModel
                    if (MainGrid.DataContext is ViewModel.LoanViewModel viewModel)
                    {
                        viewModel.Amount = 0;
                        Debug.WriteLine("Amount textbox cleared, set to 0");
                    }
                }
                else if (float.TryParse(textBox.Text, out float amount))
                {
                    // No need to do anything here since binding will update the value
                    Debug.WriteLine($"Amount changed to: {amount}");
                }
            }
        }

        // clearing the combo boxes on going back from Take Loan page
        private void BackButton_TakeLoan_Click(object sender, RoutedEventArgs e)
        {
            BankAccountComboBox.SelectedIndex = -1;
            //AmountTextBox.Text = string.Empty;
            MonthsComboBox.SelectedIndex = -1;
        }

        // clearing the combo boxes on going back from Pay Loan page
        private void BackButton_PayLoan_Click(object sender, RoutedEventArgs e)
        {
            PayBankAccountComboBox.SelectedIndex = -1;
            LoanToPayComboBox.SelectedIndex = -1;
            Debug.WriteLine("Pay Loan page - Back button clicked, ComboBoxes cleared manually");
        }
    }
}