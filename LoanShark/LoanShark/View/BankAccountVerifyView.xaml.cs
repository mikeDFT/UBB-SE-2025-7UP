using LoanShark.ViewModel;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountVerifyView : Window
    {
        BankAccountVerifyViewModel viewModel;
        public BankAccountVerifyView(string IBAN)
        {
            this.InitializeComponent();
            viewModel = new BankAccountVerifyViewModel(IBAN);
            MainGrid.DataContext = viewModel;

            viewModel.OnClose = () => this.Close();
        }
    }
}
