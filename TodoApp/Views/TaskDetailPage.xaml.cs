using System.Windows.Controls;
using TodoApp.ViewModels;

namespace TodoApp.Views
{
    /// <summary>
    /// Interaction logic for TaskDetailPage.xaml
    /// </summary>
    public partial class TaskDetailPage : Page
    {
        public TaskDetailPage(TaskDetailViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}