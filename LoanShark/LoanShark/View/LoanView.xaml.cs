using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LoanShark.ViewModel;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using LoanShark.Helper;

namespace LoanShark
{
    public partial class LoanView : Window
    {
        public LoanView()
        {
            InitializeComponent();
            var viewModel = new LoanViewModel();
            MainGrid.DataContext = viewModel;

            viewModel.CloseAction = () => this.Close(); // the closing action

            WindowManager.RegisterWindow(this);

            // Resizing window to see the whole table
            ResizeWindow(1800, 900);
        }

        private void ResizeWindow(int width, int height)
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
                if (MainGrid.DataContext is ViewModel.LoanViewModel viewModel)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        // If text is empty, set Amount to 0 in the ViewModel
                        viewModel.Amount = 0;
                        Debug.WriteLine("Amount textbox cleared, set to 0");
                    }
                    else if (decimal.TryParse(textBox.Text, out decimal amount))
                    {
                        // Change the amount, the amount to pay will be updated automatically, check the getter in the ViewModel
                        viewModel.Amount = amount;
                        Debug.WriteLine($"Amount changed to: {amount}");
                    }
                }
            }
        }

        // clearing the combo boxes on going back from Take Loan page
        private void BackButton_TakeLoan_Click(object sender, RoutedEventArgs e)
        {
            BankAccountComboBox.SelectedIndex = -1;
            // AmountTextBox.Text = string.Empty;
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