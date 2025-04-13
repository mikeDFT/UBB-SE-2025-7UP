using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using LoanShark.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class BankAccountDetailsView : Window
    {
        private BankAccountDetailsViewModel viewModel;
        public BankAccountDetailsView()
        {
            this.InitializeComponent();
            this.Activate();
            viewModel = new BankAccountDetailsViewModel();
            MainGrid.DataContext = viewModel;

            viewModel.OnClose = () => this.Close();

            WindowManager.RegisterWindow(this);
        }
    }
}
