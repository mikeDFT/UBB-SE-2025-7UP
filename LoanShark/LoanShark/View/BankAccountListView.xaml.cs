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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountListView : Window
    {
        public ObservableCollection<ItemModel> Items { get; set; }

        public BankAccountListView()
        {
            this.InitializeComponent();
            Items = new ObservableCollection<ItemModel>
            {
                new ItemModel { Name = "Item 1", IsChecked = false },
                new ItemModel { Name = "Item 2", IsChecked = true },
                new ItemModel { Name = "Item 3", IsChecked = false },
                new ItemModel { Name = "Item 4", IsChecked = true },
                new ItemModel { Name = "Item 5", IsChecked = false }
            };
        }
    }
}
