using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// A base view model class that has <see cref="RelayCommand"/> and event methods for the <see cref="IUserTaskService"/> <see cref="INavigationService"/>
    /// </summary>
    public class TaskCollectionViewModelBase : ObservableObject
    {
        protected static IUserTaskService _userTaskService;
        protected static INavigationService _navigationService;
        private static RelayCommandAsync<UserTask> updateTaskCommand = new(UpdateTask);

        //having a backing property will force the application to run only 1 of this command at a time.
        public static RelayCommandAsync<UserTask> UpdateTaskCommand
        {
            get => updateTaskCommand;
            set => updateTaskCommand = value;
        }

        public static RelayCommand<UserTask> OpenTaskCommand => new(OpenTask);
        public static RelayCommandAsync<UserTask> DeleteTaskCommand  => new(DeleteTaskAsync);
        public static RelayCommandAsync<UserTask> ToggleImportantCommand => new(ToggleImportant);
        public static RelayCommandAsync SaveChangesCommand => new(SaveChangesAsync);


        /// <summary>
        /// Creates an instance of <see cref="TaskCollectionViewModelBase"/>
        /// </summary>
        public TaskCollectionViewModelBase(IUserTaskService userTaskService, INavigationService navigationService)
        {
            _navigationService ??= navigationService;

            if (_userTaskService == null)
            {
                _userTaskService = userTaskService;
                _userTaskService.UserTaskAdded += OnUserTaskAdded;
                _userTaskService.UserTaskDeleted += OnUserTaskDeleted;
                _userTaskService.UserTaskUpdated += OnUserTaskUpdated;
                _userTaskService.SavedChanges += OnChangesSaved;
                _userTaskService.TaskListUpdated += OnTaskListUpdated;
                _userTaskService.TaskListAdded += OnTaskListAdded;
                _userTaskService.TaskListDeleted += OnTaskListDeleted;
            }
        }

        private static async Task UpdateTask(UserTask task, CancellationToken token = default)
        {
            await _userTaskService.UpdateAsync(task, token);
        }
        private static async Task DeleteTaskAsync(UserTask? t, CancellationToken token = default)
        {
            if (t == null)
                return;

            await _userTaskService.DeleteAsync(t, token);
        }

        private static async Task SaveChangesAsync(CancellationToken token = default)
        {
            await _userTaskService.SaveChangesAsync(token);
        }

        private static async Task ToggleImportant(UserTask? task, CancellationToken token = default)
        {
            if (task == null)
                return;

            task.IsImportant = !task.IsImportant;
            await SaveChangesAsync(token);
        }

        private static void OpenTask(UserTask? obj)
        {
            if (obj == null)
                return;

            INavigationParameters parameters = new NavigationParameters
            {
                { nameof(UserTask), obj }
            };
            _navigationService.Navigate("TaskDetailRegion", ViewType.TaskDetails, parameters);
        }

        protected static void AddToList<T>(ObservableCollection<T> list, T objectToAdd)
        {
            if (list == null || objectToAdd == null)
                return;

            list.Add(objectToAdd);
        }

        protected static void RemoveFromList<T>(ObservableCollection<T> list, T objectToRemove)
        {
            if (list == null || objectToRemove == null)
                return;

            list.Remove(objectToRemove);
        }
        protected virtual void OnUserTaskUpdated(object? sender, AddingNewEventArgs e)
        {
            UserTask task = (UserTask)e.NewObject;

            //explicitly notifying that the steps property might have changed since binding to collection does not triggger when property changed.
            task?.OnPropertyChanged(nameof(task.Steps).Split('.').Last());
        }
    }
}
