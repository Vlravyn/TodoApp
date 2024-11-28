using CommunityToolkit.Mvvm.ComponentModel;
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
    public class AllTasksViewModel : ObservableObject, INavigationAware
    {
        private readonly IUserTaskService _userTaskService;
        private readonly INavigationService _navigationService;

        /// <summary>
        /// This property contains all the tasks.
        /// </summary>
        public ObservableCollection<UserTask> AllTasks { get; set; } = [];

        public RelayCommandAsync AddNewTaskCommand => new(AddNewTask);

        /// <summary>
        /// Creates an instance of <see cref="AllTasksViewModel"/>
        /// </summary>
        public AllTasksViewModel(IUserTaskService userTaskService, INavigationService navigationService)
        {
            _userTaskService = userTaskService;
            _navigationService = navigationService;
            _userTaskService.UserTaskAdded += OnUserTaskAdded;
            _userTaskService.UserTaskDeleted += OnUserTaskDeleted;
            _userTaskService.SavedChanges += OnChangesSaved;
        }

        /// <summary>
        /// Adds a new task and navigates to it.
        /// </summary>
        /// <param name="token">the cancellation token</param>
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

        private void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            OnPropertyChanged(nameof(AllTasks));
        }

        private void OnUserTaskAdded(object? sender, AddingNewEventArgs e)
        {
            if(e.NewObject is UserTask ut && !AllTasks.Contains(ut))
                AllTasks.Add(ut);
        }

        private void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
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