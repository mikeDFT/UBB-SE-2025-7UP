using Microsoft.UI.Xaml.Controls;
using LoanShark.ViewModel;

namespace LoanShark.View
{
    public sealed partial class SendMoneyView : Page
    {
        public SendMoneyView()
        {
            this.InitializeComponent();
            this.DataContext = new SendMoneyViewModel(this);
        }
    }
}
