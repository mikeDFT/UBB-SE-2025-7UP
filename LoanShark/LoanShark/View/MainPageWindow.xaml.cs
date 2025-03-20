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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageWindow : Window
    {
        public MainPageWindow()
        {
            this.InitializeComponent();
        }

        private void AccountSettingsButtonHandler(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void LogOutButtonHandler(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void ExitLoanSharkButtonHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
