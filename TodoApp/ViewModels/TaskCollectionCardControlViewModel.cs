using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskCollectionCardControlViewModel : TaskCollectionViewModelBase
    {
        private static bool isSubscribedToUserTaskServiceEvents = false;
        public UserTask UserTask { get; set; }
        public static ObservableCollection<TaskList> TaskLists { get; set; }

        public RelayCommandAsync<TaskList> AddToTaskListCommand => new(AddToTaskList);
        public RelayCommandAsync OnLoadedCommand => new(OnLoaded);

        public TaskCollectionCardControlViewModel(IUserTaskService userTaskService, INavigationService navigationService)
            : base(userTaskService, navigationService)
        {
            //prevents subscribing multiple times
            if(isSubscribedToUserTaskServiceEvents is false)
            {
                _userTaskService.TaskListAdded += OnTaskListAdded;
                _userTaskService.TaskListDeleted += OnTaskListDeleted;
                isSubscribedToUserTaskServiceEvents = true;
            }
        }

        private static void OnTaskListDeleted(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is TaskList list && !TaskLists.Contains(list))
                RemoveFromList(TaskLists, list);
        }
        private static void OnTaskListAdded(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is TaskList list && !TaskLists.Contains(list))
                AddToList(TaskLists, list);
        }

        private async Task OnLoaded(CancellationToken token = default)
        {
            if(TaskLists is null)
            {
                TaskLists = new(await _userTaskService.GetAllTaskListsAsync(token));
                OnPropertyChanged(nameof(TaskLists));
            }
        }
        private async Task AddToTaskList(TaskList? list, CancellationToken token = default)
        {
            if (list == null || UserTask == null)
                return;

            list.Tasks ??= [];
            list.Tasks.Add(UserTask);
            await _userTaskService.UpdateAsync(list, token);
        }
    }
}
