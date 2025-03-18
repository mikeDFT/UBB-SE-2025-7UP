using Microsoft.UI.Xaml.Controls;
using LoanShark.ViewModel;

namespace LoanShark.View
{
    public sealed partial class CurrencyExchangeTableView : Page
    {
        public CurrencyExchangeTableView()
        {
            this.InitializeComponent();
            this.DataContext = new CurrencyExchangeTableViewModel(this);
        }
    }
}
