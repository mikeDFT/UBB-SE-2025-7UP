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
        private readonly TransactionsViewModel viewModel;
        private AppWindow appWindow;

        public TransactionsView()
        {
            this.InitializeComponent();
            viewModel = new TransactionsViewModel();

            MainGrid.DataContext = viewModel;

            viewModel.CloseAction = CloseWindow;

            WindowManager.RegisterWindow(this);

            InitializeWindow();
        }

        private void InitializeWindow()
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                appWindow.Resize(new Windows.Graphics.SizeInt32(1000, 800));
            }
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}