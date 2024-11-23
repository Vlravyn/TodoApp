using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.WPF.Navigation;

namespace TodoApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly ReminderService reminderService;

        public MainWindowViewModel(INavigationService navigationService, ReminderService reminderService)
        {
            this.navigationService = navigationService;
            this.reminderService = reminderService;
            this.reminderService.StartAsync(CancellationToken.None);
            this.reminderService.ReminderTimeReached += ReminderService_ReminderTimeReached;
        }

        private void ReminderService_ReminderTimeReached(object? sender, ReminderEventArgs e)
        {
            var notification = new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText($"Reminder for {e.UserTask.Title}");

            if (e.UserTask.DueDate is not null)
                notification.AddText($"Don't forget to complete it by {e.UserTask.DueDate}");
            notification.Show();
        }

        public RelayCommand<Enum> NavigateCommand => new RelayCommand<Enum>(Navigate);

        private void Navigate(Enum? @enum)
        {
            if (@enum is ViewType viewType)
                navigationService.Navigate("mainRegion", viewType, new NavigationParameters());
        }
    }
}