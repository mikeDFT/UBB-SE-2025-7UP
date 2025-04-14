using System.Diagnostics;
using Microsoft.UI.Xaml;
using LoanShark.View;
using LoanShark.Data;

namespace LoanShark
{
    public partial class App : Application
    {
        public App()
        {
            Debug.Print("Application is now opening...");
            this.InitializeComponent();
            DataLink.Instance.OpenConnection();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            loginWindow = new LoginView();
            loginWindow.Activate();
        }

        private LoginView? loginWindow;
    }
}
