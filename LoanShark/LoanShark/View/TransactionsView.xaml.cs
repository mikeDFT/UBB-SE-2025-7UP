using Microsoft.UI.Xaml;
using LoanShark.ViewModel;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using LoanShark.Helper;

namespace LoanShark.View
{
    public sealed partial class TransactionsView : Window
    {
        private readonly TransactionsViewModel _viewModel;
        private AppWindow _appWindow;

        public TransactionsView()
        {
            this.InitializeComponent();
            _viewModel = new TransactionsViewModel();

            MainGrid.DataContext = _viewModel;

            _viewModel.CloseAction = CloseWindow;

            WindowManager.RegisterWindow(this);

            InitializeWindow();
        }

        private void InitializeWindow()
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            if (_appWindow != null)
            {
                _appWindow.Resize(new Windows.Graphics.SizeInt32(1000, 800));
            }
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}