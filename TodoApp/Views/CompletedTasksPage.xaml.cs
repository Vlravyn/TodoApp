using System.Windows.Controls;
using System.Windows.Documents;
using TodoApp.ViewModels;

namespace TodoApp.Views
{
    /// <summary>
    /// Interaction logic for CompletedTasksPage.xaml
    /// </summary>
    public partial class CompletedTasksPage : Page
    {
        public CompletedTasksPage(CompletedTasksViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}