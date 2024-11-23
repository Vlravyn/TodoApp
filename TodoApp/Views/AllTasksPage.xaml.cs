using TodoApp.ViewModels;

namespace TodoApp.Views
{
    /// <summary>
    /// Interaction logic for AllTasksPage.xaml
    /// </summary>
    public partial class AllTasksPage
    {
        public AllTasksPage(AllTasksViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}