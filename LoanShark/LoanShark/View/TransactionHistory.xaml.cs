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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionHistory : Window
    {

        public ObservableCollection<string> TransactionsForMenu { get; set; }
        public ObservableCollection<string> TransactionsDetailed { get; set; }
        public TransactionHistory()
        {
            this.InitializeComponent();

            this.TransactionsForMenu = new ObservableCollection<string>();
            this.TransactionsDetailed = new ObservableCollection<string>();
            try
            {
                // Create an instance of the DataLink class
                DataLink dataLink = new DataLink();

                // Define the stored procedure name
                string storedProcedure = "GetTransactionsByType";

                // Define the parameters for the stored procedure, if any
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TransactionType", SqlDbType.NVarChar) { Value = "Payment" }
                };

                // Call the ExecuteReader method
                DataTable result = dataLink.ExecuteReader(storedProcedure, parameters);

                // Process the result
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    this.TransactionsForMenu.Add(transaction.tostringForMenu());
                    this.TransactionsDetailed.Add(transaction.tostringDetailed());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            this.InitializeComponent();

        }

        


    }
}
