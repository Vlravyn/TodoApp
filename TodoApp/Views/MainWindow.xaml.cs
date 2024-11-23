using TodoApp.ViewModels;
using Wpf.Ui.Controls;

namespace TodoApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}