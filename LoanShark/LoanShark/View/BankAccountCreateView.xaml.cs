using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows;
using LoanShark.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class BankAccountCreateView : Window
    {
        private BankAccountCreateViewModel viewModel;
        public BankAccountCreateView(int userID)
        {
            this.InitializeComponent();
            viewModel = new BankAccountCreateViewModel(userID);
            MainGrid.DataContext = viewModel;
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           if(sender is RadioButton radioButton)
           {
                viewModel.SelectedItem = radioButton.Content.ToString();    
           }
        }
    }
}
