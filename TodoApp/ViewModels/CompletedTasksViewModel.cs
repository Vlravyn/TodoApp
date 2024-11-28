using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class CompletedTasksViewModel : ObservableObject, INavigationAware
    {
        private readonly IUserTaskService _userTaskService;

        public ObservableCollection<UserTask> CompletedTasks { get; set; } = [];

        /// <summary>
        /// Creates an instance off <see cref="CompletedTasksViewModel"/>
        /// </summary>
        public CompletedTasksViewModel(IUserTaskService userTaskService)
        {
            _userTaskService = userTaskService;
            _userTaskService.SavedChanges += OnChangesSaved;
            _userTaskService.UserTaskDeleted += OnUserTaskDeleted;
            _userTaskService.UserTaskUpdated += OnUserTaskUpdated;
        }

        private void OnUserTaskUpdated(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is UserTask task)
            {
                if (CompletedTasks.Contains(task))
                {
                    if (task.IsCompleted is false)
                        CompletedTasks.Remove(task);
                }
                else
                {
                    if (task.IsCompleted)
                        CompletedTasks.Add(task);
                }
            }
        }
        private void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is UserTask userTask && CompletedTasks.Contains(e.NewObject))
                CompletedTasks.Remove(userTask);
        }
        private void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            OnPropertyChanged(nameof(CompletedTasks));
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if(CompletedTasks is null ||  CompletedTasks.Count == 0)
            {
                CompletedTasks = new ObservableCollection<UserTask>(await _userTaskService.GetCompletedUserTasksAsync());
                OnPropertyChanged(nameof(CompletedTasks));
            }
        }

        public void OnNavigatedFrom()
        {
        }

    }
}