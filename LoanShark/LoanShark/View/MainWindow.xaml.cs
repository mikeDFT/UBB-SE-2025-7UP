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
using System.Diagnostics;
using LoanShark.Data;
using LoanShark.Helper;
using Microsoft.Data.SqlClient;
using System.Data;
using LoanShark.Domain;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public ObservableCollection<Transaction> Transactions { get; set; }

        public MainWindow()
        {
            this.Transactions = new ObservableCollection<Transaction>();
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
                    this.Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = AppConfig.GetConnectionString("MyLocalDb");
            try
            {
                DataLink dataLink = new DataLink();
                dataLink.OpenConnection();
                dataLink.CloseConnection();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
    }
}
