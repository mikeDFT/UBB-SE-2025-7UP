using LoanShark.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;

namespace LoanShark.View
{
    public sealed partial class SendMoneyView : Window
    {
        private readonly SendMoneyViewModel _viewModel;
        private AppWindow _appWindow;

        public SendMoneyView()
        {
            this.InitializeComponent();
            _viewModel = new SendMoneyViewModel();
            MainGrid.DataContext = _viewModel;
            _viewModel.CloseAction = CloseWindow;

            InitializeWindow(800, 600);
        }

        private void CloseWindow()
        {
            _appWindow.Destroy();
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
