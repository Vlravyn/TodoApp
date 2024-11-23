using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class CompletedTasksViewModel : TaskCollectionViewModelBase, INavigationAware
    {
        #region Public Properties

        public ObservableCollection<UserTask> CompletedTasks { get; set; } = new();

        #endregion Public Properties

        #region Constructors

        public CompletedTasksViewModel(IUserTaskService userTaskService, INavigationService navigationService)
            : base(userTaskService, navigationService)
        {
        }

        #endregion Constructors

        #region Private Methods

        protected override void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskDeleted(sender, e);
            if (sender is UserTask task && CompletedTasks.Contains(task))
                CompletedTasks.Remove(task);
        }

        protected override void OnUserTaskUpdated(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskUpdated(sender , e);
            if (sender is UserTask task && task.IsCompleted && !CompletedTasks.Contains(task))
                CompletedTasks.Add(task);
        }

        protected override void OnUserTaskAdded(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskAdded(sender, e);
            if (sender is UserTask task && task.IsCompleted)
                CompletedTasks.Add(task);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            CompletedTasks = new ObservableCollection<UserTask>(await userTaskService.GetCompletedUserTasksAsync());
        }

        public void OnNavigatedFrom()
        {
        }

        #endregion Private Methods
    }
}