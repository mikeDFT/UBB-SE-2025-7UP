using System;
using System.Threading.Tasks;
using LoanShark.Helper;
using LoanShark.ViewModel;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinRT.Interop;

namespace LoanShark.View
{
    public sealed partial class SendMoneyView : Window
    {
        public SendMoneyViewModel ViewModel { get; private set; }

        private AppWindow appWindow;

        public SendMoneyView()
        {
            this.InitializeComponent();
            this.ViewModel = new SendMoneyViewModel();
            MainGrid.DataContext = this.ViewModel;
            this.ViewModel.CloseAction = CloseWindow;

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

        public async void SendMoneyButtonHandler(object sender, RoutedEventArgs e)
        {
            string result = await ViewModel.ProcessPaymentAsync();
            await ShowDialog(result);
        }

        private async Task ShowDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Transaction Result",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}
