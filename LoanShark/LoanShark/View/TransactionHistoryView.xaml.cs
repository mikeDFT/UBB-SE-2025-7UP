using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using LoanShark.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionHistoryView : Window
    {
        public TransactionsHistoryViewModel transactionsViewModel;
        public ObservableCollection<string> currentList;
        private bool isSortedAscending = true;
        
        public TransactionHistoryView()
        {
            this.transactionsViewModel = new TransactionsHistoryViewModel();
            this.InitializeComponent();
            SortAscendingButton.IsChecked = true;
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            currentList = await transactionsViewModel.retrieveForMenu();
            TransactionList.ItemsSource = currentList;
        }

        private void ExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            transactionsViewModel.CreateCSV();
            ContentDialog dialog = new ContentDialog
            {
                Title = "Export Complete",
                Content = "Transactions exported to CSV on Desktop.",
                CloseButtonText = "Ok",
                XamlRoot = this.Content.XamlRoot
            };
            _ = dialog.ShowAsync();
        }

        private async void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            if (isSortedAscending == false)
            {
                isSortedAscending = true;
                SortAscendingButton.IsChecked = true;
                SortDescendingButton.IsChecked = false;
                currentList = await transactionsViewModel.SortByDate("Ascending");
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                SortAscendingButton.IsChecked = true;
            }
        }

        private async void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            if (isSortedAscending == true)
            {
                isSortedAscending = false;
                SortAscendingButton.IsChecked = false;
                SortDescendingButton.IsChecked = true;
                currentList = await transactionsViewModel.SortByDate("Descending");
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                SortDescendingButton.IsChecked = true;
            }
        }

        private async void TransactionTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedText = (sender as TextBox).Text;
            if (!string.IsNullOrEmpty(typedText))
            {
                currentList = await transactionsViewModel.FilterByTypeForMenu(typedText);
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                currentList = await transactionsViewModel.retrieveForMenu();
                TransactionList.ItemsSource = currentList;
            }
        }

        private async void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionList.SelectedItem != null)
            {
                string selectedTransactionForMenu = TransactionList.SelectedItem as string;
                // Retrieve the detailed information of the selected transaction
                var selectedTransaction = await transactionsViewModel.GetTransactionByMenuString(selectedTransactionForMenu);
                string detailedTransaction = selectedTransaction.TostringDetailed();
                TransactionDetailsView transactionDetailsWindow = new TransactionDetailsView(detailedTransaction, selectedTransaction);
                transactionDetailsWindow.Activate();
            }
        }

        private async void ViewGraphics_Click(object sender, RoutedEventArgs e)
        {
            var transactionTypeCounts = await transactionsViewModel.GetTransactionTypeCounts();
            TransactionHistoryGraphicalRepresentationView transactionGraphicsWindow = new TransactionHistoryGraphicalRepresentationView(transactionTypeCounts);
            transactionGraphicsWindow.Activate();
        }
    }
}