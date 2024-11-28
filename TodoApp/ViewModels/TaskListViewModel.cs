using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskListViewModel : ObservableObject, INavigationAware
    {
        private TaskList _currentTaskList;
        private readonly IUserTaskService userTaskService;

        public ObservableCollection<UserTask> TasksOnThisList { get; set; }

        public TaskList CurrentTaskList
        {
            get => _currentTaskList;
            set => SetProperty(ref _currentTaskList, value);
        }
        public RelayCommandAsync SaveChangesCommand => new(SaveChangesAsync);

        private async Task SaveChangesAsync(CancellationToken token)
        {
            await userTaskService.SaveChangesAsync(token);
        }

        public TaskListViewModel(IUserTaskService userTaskService)
        {
            this.userTaskService = userTaskService;
            userTaskService.TaskListUpdated += OnTaskListUpdated;
        }

        private void OnTaskListUpdated(object? sender, AddingNewEventArgs e)
        {
            CurrentTaskList = e.NewObject as TaskList;
            OnPropertyChanged(nameof(CurrentTaskList));
            OnPropertyChanged(nameof(CurrentTaskList.Title));
            OnPropertyChanged(nameof(CurrentTaskList.Tasks));
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var tl = parameters.FirstOrDefault(t => t.Key == nameof(TaskList));
            if(tl.Value is TaskList taskList)
            {
                CurrentTaskList = taskList;
                OnPropertyChanged(nameof(CurrentTaskList));
                OnPropertyChanged(nameof(CurrentTaskList.Title));
                OnPropertyChanged(nameof(CurrentTaskList.Tasks));
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
