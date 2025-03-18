using Microsoft.UI.Xaml.Controls;
using LoanShark.ViewModel;

namespace LoanShark.View
{
    public sealed partial class TransactionsView : Page
    {
        public TransactionsView()
        {
            this.InitializeComponent();
            this.DataContext = new TransactionsViewModel();
        }
    }
}
