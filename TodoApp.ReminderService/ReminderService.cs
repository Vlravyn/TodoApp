using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.Reminder
{
    public class ReminderService : BackgroundService
    {
        private readonly IUserTaskService userTaskService;
        private readonly ILogger<ReminderService> _logger;
        private List<ReminderData> Reminders = new();

        public event EventHandler<ReminderEventArgs> ReminderTimeReached;
        public event EventHandler<ReminderEventArgs> DueDateReached;


        public ReminderService(IUserTaskService userTaskService, ILogger<ReminderService> logger)
        {
            this.userTaskService = userTaskService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var storedTasks = await userTaskService.GetAllUserTasksAsync(stoppingToken);

            foreach (var task in storedTasks)
            {
                if (task is null)
                    continue;

                AddNewReminderData(this, task);
                foreach (var reminder in Reminders)
                    CreateReminderDataTimers(reminder);
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"ReminderService started at {DateTime.Now}");

                await Task.Delay(new TimeSpan(1, 0, 0), stoppingToken);

                var dbTasks = await userTaskService.GetAllUserTasksAsync(stoppingToken);
                var newTasks = dbTasks.Where(t => !Reminders.Any(k => k.UserTask == t));


                foreach (var newTask in newTasks)
                    AddNewReminderData(this, newTask);

                foreach (var reminder in Reminders)
                    CreateReminderDataTimers(reminder);
            }
        }

        /// <summary>
        /// Creates new ReminderData for the given service
        /// </summary>
        /// <param name="service">the service to add the reminder data to</param>
        /// <param name="userTask">the <see cref="UserTask"/> to create the <see cref="ReminderData"/> of</param>
        private static void AddNewReminderData(ReminderService service, UserTask userTask)
        {
            var reminder = new ReminderData(userTask);
            reminder.RemindUserReached += (_, e) => service.ReminderTimeReached?.Invoke(service, e);
            reminder.DueDateReached += (_, e) => service.DueDateReached?.Invoke(service, e);
            service.Reminders.Add(reminder);

            userTask.PropertyChanged += (_, e) => UserTask_PropertyChanged(reminder, e);
        }

        /// <summary>
        /// Creates the <see cref="ReminderData.RemindUserTimer"/> and <see cref="ReminderData.DueDateTimer"/> and starts them if less then 1 hour is left to reach the time of properties in <see cref="ReminderData.UserTask"/>
        /// </summary>
        /// <param name="sender">the <see cref="ReminderData"/> to set the timers of</param>
        private static void CreateReminderDataTimers(ReminderData sender)
        {
            var task = sender.UserTask;
            var utcTime = DateTime.UtcNow;
            if (utcTime < task.RemindUserUtc)
            {
                var test = task.RemindUser;
                var timeleft = task.RemindUserUtc - utcTime;

                //The service is checking for timer updates every hour anyway, so we do not need to start the timer early
                if (timeleft.Value.TotalHours < 1)
                    ReminderData.SetRemindUserTimer(sender, timeleft.Value.Ticks);
            }
            if (utcTime < task.DueDateUtc)
            {
                var timeleft = task.DueDateUtc - utcTime;

                if (timeleft.Value.TotalHours < 1)
                    ReminderData.SetDueDateTimer(sender, timeleft.Value.Ticks);
            }
        }

        private static void UserTask_PropertyChanged(ReminderData? sender, PropertyChangedEventArgs e)
        {
            if (sender is not null && (e.PropertyName == nameof(UserTask.RemindUser) || e.PropertyName == nameof(UserTask.DueDate)))
                CreateReminderDataTimers(sender);
        }
    }
}
