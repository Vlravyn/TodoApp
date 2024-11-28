using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
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
            var task = await _userTaskService.AddAsync(new UserTask()
            {
                Title = "New Task",
            }, token);

            _navigationService.Navigate("TaskDetailRegion", ViewType.TaskDetails, new NavigationParameters()
            {
                { nameof(UserTask), task }
            });
        }

        protected override void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            base.OnChangesSaved(sender, e);
            OnPropertyChanged(nameof(AllTasks));
        }

        protected override void OnUserTaskAdded(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskAdded(sender, e);
            if(e.NewObject is UserTask ut && !AllTasks.Contains(ut))
                AllTasks.Add(ut);
        }

        protected override void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskDeleted(sender, e);
            if (e.NewObject is UserTask ut && AllTasks.Contains(ut))
                AllTasks.Remove(ut);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            AllTasks = new(await _userTaskService.GetAllUserTasksAsync(CancellationToken.None));
            OnPropertyChanged(nameof(AllTasks));
        }

        public void OnNavigatedFrom()
        {
        }
    }
}