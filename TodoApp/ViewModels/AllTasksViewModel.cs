using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using System.Collections.ObjectModel;
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

        protected override async void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            base.OnChangesSaved(sender, e);
            await UpdateTasksCollection();
        }

        private async Task UpdateTasksCollection()
        {
            AllTasks = new(await userTaskService.GetAllUserTasksAsync(CancellationToken.None));
            OnPropertyChanged(nameof(AllTasks));
        }
        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await UpdateTasksCollection();
        }

        public void OnNavigatedFrom()
        {
        }
    }
}