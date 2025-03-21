using Microsoft.UI.Xaml;
using LoanShark.View;

namespace LoanShark
{
    public partial class App : Application
    {
        public static Window MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new TransactionsView(); 
            MainWindow.Activate();
        }
    }
}
