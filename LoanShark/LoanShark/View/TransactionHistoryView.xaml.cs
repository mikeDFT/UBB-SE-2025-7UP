using System.Collections.ObjectModel;
using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionHistoryView : Window
    {
        public TransactionsHistoryViewModel TransactionsViewModel;
        public ObservableCollection<string> CurrentList;
        private bool isSortedAscending = true;

        public TransactionHistoryView()
        {
            this.TransactionsViewModel = new TransactionsHistoryViewModel();
            this.InitializeComponent();
            SortAscendingButton.IsChecked = true;
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            CurrentList = await TransactionsViewModel.RetrieveForMenu();
            TransactionList.ItemsSource = CurrentList;
        }

        private void ExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            TransactionsViewModel.CreateCSV();
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
                CurrentList = await TransactionsViewModel.SortByDate("Ascending");
                TransactionList.ItemsSource = CurrentList;
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
                CurrentList = await TransactionsViewModel.SortByDate("Descending");
                TransactionList.ItemsSource = CurrentList;
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
                CurrentList = await TransactionsViewModel.FilterByTypeForMenu(typedText);
                TransactionList.ItemsSource = CurrentList;
            }
            else
            {
                CurrentList = await TransactionsViewModel.RetrieveForMenu();
                TransactionList.ItemsSource = CurrentList;
            }
        }

        private async void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionList.SelectedItem != null)
            {
                string selectedTransactionForMenu = TransactionList.SelectedItem as string;
                // Retrieve the detailed information of the selected transaction
                var selectedTransaction = await TransactionsViewModel.GetTransactionByMenuString(selectedTransactionForMenu);
                string detailedTransaction = selectedTransaction.TostringDetailed();
                TransactionDetailsView transactionDetailsWindow = new TransactionDetailsView(detailedTransaction, selectedTransaction);
                transactionDetailsWindow.Activate();
            }
        }

        private async void ViewGraphics_Click(object sender, RoutedEventArgs e)
        {
            var transactionTypeCounts = await TransactionsViewModel.GetTransactionTypeCounts();
            TransactionHistoryGraphicalRepresentationView transactionGraphicsWindow = new TransactionHistoryGraphicalRepresentationView(transactionTypeCounts);
            transactionGraphicsWindow.Activate();
        }
    }
}