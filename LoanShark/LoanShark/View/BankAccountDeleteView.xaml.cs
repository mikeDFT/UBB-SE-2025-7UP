using LoanShark.ViewModel;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountDeleteView : Window
    {
        public BankAccountDeleteView(string IBAN)
        {
            this.InitializeComponent();
            var viewModel = new BankAccountDeleteViewModel(IBAN);
            MainGrid.DataContext = viewModel;

            viewModel.onClose = () => this.Close();
        }
    }
}
