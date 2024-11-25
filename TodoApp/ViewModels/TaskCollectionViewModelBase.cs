using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using MvvmEssentials.WPF.Navigation;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskCollectionViewModelBase : ObservableObject
    {
        protected readonly IUserTaskService userTaskService;
        protected readonly INavigationService navigationService;

        public TaskCollectionViewModelBase(IUserTaskService userTaskService, INavigationService navigationService)
        {
            this.userTaskService = userTaskService;
            this.navigationService = navigationService;

            this.userTaskService.UserTaskAdded += OnUserTaskAdded;
            this.userTaskService.UserTaskDeleted += OnUserTaskDeleted;
            this.userTaskService.UserTaskUpdated += OnUserTaskUpdated;
            this.userTaskService.UserTaskSavedChanges += OnChangesSaved;
        }

        #region ICommands

        public RelayCommand<UserTask> OpenTaskCommand => new(OpenTask);
        public RelayCommandAsync<UserTask> DeleteTaskCommand  => new(DeleteTaskAsync);
        public RelayCommandAsync<UserTask> MarkAsImportantCommand => new(MarkAsImportant);
        public RelayCommandAsync SaveChangesCommand => new(SaveChangesAsync);

        #endregion ICommands

        #region Methods

        protected virtual void OnChangesSaved(object? sender, Microsoft.EntityFrameworkCore.SavedChangesEventArgs e)
        {
        }
        protected virtual void OnUserTaskUpdated(object? sender, AddingNewEventArgs e)
        {
            UserTask task = (UserTask)e.NewObject;

            //explicitly notifying that the steps property might have changed since binding to collection does not triggger when property changed.
            task?.OnPropertyChanged(nameof(task.Steps).Split('.').Last());
        }

        protected virtual void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
        }

        protected virtual void OnUserTaskAdded(object? sender, AddingNewEventArgs e)
        {
        }

        private async Task DeleteTaskAsync(UserTask? t, CancellationToken token = default)
        {
            if (t == null)
                return;

            await userTaskService.DeleteAsync(t, token);
        }

        private async Task SaveChangesAsync(CancellationToken token = default)
        {
            await userTaskService.SaveChangesAsync(token);
        }

        private async Task MarkAsImportant(UserTask? task, CancellationToken token = default)
        {
            if (task == null)
                return;

            task.IsImportant = true;
            await SaveChangesAsync(token);
        }

        private void OpenTask(UserTask? obj)
        {
            if (obj == null)
                return;

            INavigationParameters parameters = new NavigationParameters()
            {
                { nameof(UserTask), obj }
            };
            navigationService.Navigate("TaskDetailRegion", ViewType.TaskDetails, parameters);
        }

        #endregion
    }
}
