using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LoanShark.Domain;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Microsoft.UI;
using System;

namespace LoanShark
{
    // this displays the details of a transaction, no biggie
    public sealed partial class TransactionDetails : Window
    {
        private Transaction _transaction;
        private AppWindow _appWindow;

        public TransactionDetails(string transactionDetails, Transaction transaction)
        {
            this.InitializeComponent();
            TransactionDetailsTextBlock.Text = transactionDetails;
            _transaction = transaction;

            // Get the AppWindow associated with this Window
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            _appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));

            ResizeWindow(800, 600);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            string newDescription = DescriptionTextBox.Text;
            if (!string.IsNullOrEmpty(newDescription))
            {
                _transaction.TransactionDescription = newDescription;
                TransactionsHistoryVM.UpdateTransactionDescription(_transaction.TransactionID, newDescription);
                TransactionDetailsTextBlock.Text = _transaction.tostringDetailed();
            }
        }

        public void ResizeWindow(int width, int height)
        {
            if (_appWindow != null)
            {
                _appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
            }
        }
    }
}