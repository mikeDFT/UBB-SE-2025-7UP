using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountListView : Window
    {
        BankAccountListViewModel viewModel;
        Boolean isDoubleClicked = false;
        public BankAccountListView(int userID)
        {
            this.InitializeComponent();
            viewModel = new BankAccountListViewModel(userID);
            MainGrid.DataContext = viewModel;

            viewModel.OnClose = () => this.Close();
        }
    }
}
