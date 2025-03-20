using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LoanShark
{
    public sealed partial class TransactionDetails : Window
    {
        public TransactionDetails(string transactionDetails)
        {
            this.InitializeComponent();
            TransactionDetailsTextBlock.Text = transactionDetails;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}