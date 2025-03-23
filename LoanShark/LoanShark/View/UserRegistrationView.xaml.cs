using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using LoanShark.Repository;
using LoanShark.Domain;
using LoanShark.ViewModel;
using Windows.System;
using LoanShark.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserRegistrationView : Window
    {
        public UserRegistrationView()
        {
            this.InitializeComponent();
            var viewModel = new UserRegistrationViewModel();
            viewModel.CloseAction = () => 
            {
                LoginView loginWindow = new LoginView();
                loginWindow.Activate();
                this.Close();
            };
            MainPanel.DataContext = viewModel;
            WindowManager.RegisterWindow(this);
        }
    }
}
