using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskCollectionCardControlViewModel : TaskCollectionViewModelBase
    {
        public UserTask UserTask { get; set; }
        public ObservableCollection<TaskList> TaskLists { get; set; } = new();

        public RelayCommandAsync<TaskList> AddToTaskListCommand => new(AddToTaskList);
        public RelayCommandAsync OnLoadedCommand => new(OnLoaded);

        public TaskCollectionCardControlViewModel(IUserTaskService userTaskService, INavigationService navigationService)
            : base(userTaskService, navigationService)
        {
        }

        private async Task OnLoaded(CancellationToken token = default)
        {
            TaskLists = new(await _userTaskService.GetAllTaskListsAsync());
            OnPropertyChanged(nameof(TaskLists));
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
