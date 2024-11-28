using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TodoApp.Core.DataModels;
using TodoApp.ViewModels;

namespace TodoApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TaskCollectionCardControl.xaml
    /// </summary>
    public partial class TaskCollectionCardControl : UserControl
    {
        public TaskCollectionCardControl()
        {
            InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            var ViewModel = App.GetService<TaskCollectionCardControlViewModel>();
            if(ViewModel.UserTask is null && DataContext is UserTask userTask)
            {
                ViewModel.UserTask = userTask;
                DataContext = ViewModel;
                ViewModel.OnLoadedCommand.Execute(null);
            }
        }

        private void MarkAsImportantMenuItemClicked(object sender, RoutedEventArgs e)
        {
            if(sender is MenuItem menuItem && menuItem.DataContext is UserTask userTask)
            {
                //logically this if else should be inverse, but the menu item uses Command to change the IsImportant property.
                //So the property toggles after this method is run. Hence the inverted header values
                if (userTask.IsImportant)
                    menuItem.Header = "Mark as important";
                else
                    menuItem.Header = "Mark as unimportant";
            }
        }
    }
}
