using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using LoanShark.View;
using LoanShark.Data;
using System.Diagnostics;
using LoanShark.Helper;
using LoanShark.Domain;
using System;

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
