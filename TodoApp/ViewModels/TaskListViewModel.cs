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


        public TaskListViewModel(IUserTaskService userTaskService)
        {
            this.userTaskService = userTaskService;
            userTaskService.TaskListUpdated += OnTaskListUpdated;
        }


        private async Task SaveChangesAsync(CancellationToken token)
        {
            await userTaskService.SaveChangesAsync(token);
        }

        private void OnTaskListUpdated(object? sender, AddingNewEventArgs e)
        {
            if(e.NewObject is TaskList taskList && taskList.Id == CurrentTaskList.Id)
            {
                CurrentTaskList = taskList;
                OnPropertyChanged(nameof(CurrentTaskList));
                OnPropertyChanged(nameof(CurrentTaskList.Title));
                OnPropertyChanged(nameof(CurrentTaskList.Tasks));
            }
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var pair = parameters.FirstOrDefault(t => t.Key == nameof(TaskList));

            if (pair.Equals(default(KeyValuePair<string, object>)))
                throw new ArgumentException($"The passed in parameter does not contain a {nameof(KeyValuePair)} with key {nameof(TaskList)} in {nameof(TaskDetailViewModel)}");

            if (pair.Value is not TaskList)
                throw new ArgumentException($"Unknown type was passed as parameter to {nameof(TaskDetailViewModel)} with key {nameof(TaskList)}");

            CurrentTaskList = (TaskList)pair.Value;
            OnPropertyChanged(nameof(CurrentTaskList));
            OnPropertyChanged(nameof(CurrentTaskList.Title));
            OnPropertyChanged(nameof(CurrentTaskList.Tasks));
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
