using Microsoft.UI.Xaml;
using LoanShark.View;
using LoanShark.Data;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
            m_loginWindow = new LoginView();
            m_loginWindow.Activate();
        }

        private LoginView? m_loginWindow;
    }
}
