using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.WPF.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class AllTasksViewModel : TaskCollectionViewModelBase, INavigationAware
    {
        public ObservableCollection<UserTask> AllTasks { get; set; } = new();

        public RelayCommandAsync AddNewTaskCommand => new(AddNewTask);

        public AllTasksViewModel(IUserTaskService userTaskService, INavigationService navigationService)
            : base(userTaskService, navigationService)
        {
        }

        private async Task AddNewTask(CancellationToken token = default)
        {
            var task = await userTaskService.AddAsync(new UserTask()
            {
                Title = "New Task",
            }, token);

            navigationService.Navigate("TaskDetailRegion", ViewType.TaskDetails, new NavigationParameters()
            {
                { nameof(UserTask), task }
            });
        }

        protected override void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is UserTask task)
                AllTasks.Remove(task);

            base.OnUserTaskDeleted(sender, e);
        }

        protected override void OnUserTaskAdded(object? sender, AddingNewEventArgs e)
        {
            if (e.NewObject is UserTask task)
                AllTasks.Add(task);

            base.OnUserTaskAdded(sender, e);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            AllTasks = new(await userTaskService.GetAllUserTasksAsync(CancellationToken.None));
        }

        public void OnNavigatedFrom()
        {
        }
    }
}