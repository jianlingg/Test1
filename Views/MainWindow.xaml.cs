using Wpf.Ui.Controls;
using Test1.Views.Pages;

namespace Test1.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (_, _) => RootNavigation.Navigate(typeof(DataPage));
        }
    }
}
