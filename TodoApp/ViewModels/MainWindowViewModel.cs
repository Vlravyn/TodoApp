using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.Notifications;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly ReminderService reminderService;
        private readonly IUserTaskService userTaskService;

        public ObservableCollection<TaskList> TaskLists { get; set; }


        public RelayCommand<Enum> NavigateCommand => new(Navigate);
        public RelayCommand<TaskList> OpenTaskListCommand => new(OpenTaskList);
        public RelayCommand CreateNewTaskListCommand => new(CreateNewTaskList);
        public RelayCommand OnLoadedCommand => new(OnLoaded);
        public RelayCommandAsync<TaskList> DeleteTaskListCommand => new(DeleteTaskList);

        public MainWindowViewModel(INavigationService navigationService, IUserTaskService userTaskService, ReminderService reminderService)
        {
            TaskLists = [];
            this.navigationService = navigationService;
            this.userTaskService = userTaskService;
            userTaskService.TaskListAdded += OnTaskListAdded;
            userTaskService.TaskListDeleted += OnTaskListDeleted;
            this.reminderService = reminderService;
            this.reminderService.StartAsync(CancellationToken.None);
            this.reminderService.ReminderTimeReached += OnReminderTimeReached;
        }

        private async Task DeleteTaskList(TaskList list, CancellationToken token)
        {
            await userTaskService.DeleteAsync(list, token);
        }

        private async void CreateNewTaskList()
        {
            var trackedList = await userTaskService.AddAsync(new TaskList()
            {
                Title = "Untitled list"
            });

            OpenTaskList(trackedList);
        }

        private void OpenTaskList(TaskList? list)
        {
            if (list != null)
                navigationService.Navigate("mainRegion", ViewType.TaskList, new NavigationParameters()
                {
                    { nameof(TaskList), list }
                });
        }

        private void OnTaskListDeleted(object? sender, AddingNewEventArgs e)
        {
            if(e.NewObject is TaskList taskList && TaskLists.Contains(taskList))
                TaskLists.Remove(taskList);
        }

        private void OnTaskListAdded(object? sender, AddingNewEventArgs e)
        {
            if(e.NewObject is TaskList taskList)
                TaskLists.Add(taskList);
        }

        private async void OnLoaded()
        {
            TaskLists = new ObservableCollection<TaskList>(await userTaskService.GetAllTaskListsAsync());
            OnPropertyChanged(nameof(TaskLists));
        }
        private void OnReminderTimeReached(object? sender, ReminderEventArgs e)
        {
            var notification = new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText($"Reminder for {e.UserTask.Title}");

            if (e.UserTask.DueDate is not null)
                notification.AddText($"Don't forget to complete it by {e.UserTask.DueDate}");
            notification.Show();
        }

        private void Navigate(Enum? @enum)
        {
            if (@enum is ViewType viewType)
                navigationService.Navigate("mainRegion", viewType, new NavigationParameters());
        }
    }
}