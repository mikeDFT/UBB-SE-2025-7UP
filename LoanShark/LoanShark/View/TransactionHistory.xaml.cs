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
using System.Collections.ObjectModel;
using LoanShark.Domain;
using LoanShark.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using LoanShark.Repository;
using LoanShark.ViewModel;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionHistory : Window
    {

        public TransactionsHistoryViewModel transactionsViewModel;
        public ObservableCollection<string> currentList;
        private bool isSortedAscending = true;
        public TransactionHistory(TransactionsHistoryViewModel transactionsViewModel)
        {
            this.transactionsViewModel = transactionsViewModel;
            currentList = transactionsViewModel.retrieveForMenu();
            this.InitializeComponent();
            SortAscendingButton.IsChecked = true;
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

        private void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            if (isSortedAscending == false)
            {
                isSortedAscending = true;
                SortAscendingButton.IsChecked = true;
                SortDescendingButton.IsChecked = false;
                currentList = transactionsViewModel.SortByDate("Ascending");
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                SortAscendingButton.IsChecked = true;
            }

        }

        private void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            if (isSortedAscending == true)
            {
                isSortedAscending = false;
                SortAscendingButton.IsChecked = false;
                SortDescendingButton.IsChecked = true;
                currentList = transactionsViewModel.SortByDate("Descending");
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                SortDescendingButton.IsChecked = true;
            }


        }

        private void TransactionTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedText = (sender as TextBox).Text;
            if (!string.IsNullOrEmpty(typedText))
            {
                currentList = transactionsViewModel.FilterByTypeForMenu(typedText);
                TransactionList.ItemsSource = currentList;
            }
            else
            {
                currentList = transactionsViewModel.retrieveForMenu();
                TransactionList.ItemsSource = currentList;
            }
        }

        private void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionList.SelectedItem != null)
            {
                string selectedTransactionForMenu = TransactionList.SelectedItem as string;
                // Retrieve the detailed information of the selected transaction
                var selectedTransaction = transactionsViewModel.GetTransactionByMenuString(selectedTransactionForMenu);
                string detailedTransaction = selectedTransaction.tostringDetailed();
                TransactionDetails transactionDetailsWindow = new TransactionDetails(detailedTransaction, selectedTransaction);
                transactionDetailsWindow.Activate();
            }
        }

        private void ViewGraphics_Click(object sender, RoutedEventArgs e)
        {
            var transactionTypeCounts = transactionsViewModel.GetTransactionTypeCounts();
            TransactionHistoryGraphicalRepresentation transactionGraphicsWindow = new TransactionHistoryGraphicalRepresentation(transactionTypeCounts);
            transactionGraphicsWindow.Activate();
        }

    }
}