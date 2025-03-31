using Microsoft.UI.Xaml;
using LoanShark.ViewModel;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using LoanShark.Helper;

namespace LoanShark.View
{
    public sealed partial class CurrencyExchangeTableView : Window
    {
        private readonly CurrencyExchangeTableViewModel _viewModel;
        private AppWindow _appWindow;

        public CurrencyExchangeTableView()
        {
            this.InitializeComponent();
            _viewModel = new CurrencyExchangeTableViewModel();

            MainGrid.DataContext = _viewModel; 

            WindowManager.RegisterWindow(this);

            _viewModel.CloseAction = CloseWindow; 

            InitializeWindow(1000,800);
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void InitializeWindow(int width, int height)
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);
            _appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        }
    }
}
