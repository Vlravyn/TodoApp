using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using TodoApp.Core.DataModels;

namespace TodoApp.Core.Services
{
    /// <summary>
    /// Sets up reminders for the <see cref="UserTask"/> stored in the database.
    /// </summary>
    public class ReminderService : BackgroundService
    {
        private readonly IUserTaskService userTaskService;
        private readonly ILogger<ReminderService> _logger;
        private List<ReminderData> Reminders = [];

        //the number of hours that this service will wait before attempting to create Timers for ReminderData again.
        private readonly int WaitHoursBeforeRefresh = 1;

        /// <summary>
        /// Raises when the <see cref="UserTask.RemindUser"/> time is reached.
        /// </summary>
        public event EventHandler<ReminderEventArgs> ReminderTimeReached;

        /// <summary>
        /// Raises when the <see cref="UserTask.DueDate"/> time is reached.
        /// </summary>
        public event EventHandler<ReminderEventArgs> DueDateReached;

        /// <summary>
        /// Creates an instance of <see cref="ReminderService"/>
        /// </summary>
        public ReminderService(IUserTaskService userTaskService, ILogger<ReminderService> logger)
        {
            this.userTaskService = userTaskService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var storedTasks = await userTaskService.GetAllUserTasksAsync(stoppingToken);
            _logger.LogInformation($"ReminderService started at {DateTime.Now}");

            //Attempt to create timers for UserTasks when executed.
            foreach (var task in storedTasks)
            {
                if (task is null)
                    continue;

                AddNewReminderData(this, task);
                foreach (var reminder in Reminders)
                    CreateReminderDataTimers(reminder, WaitHoursBeforeRefresh);
            }

            //Check if the time is close to reaching and set up timers if so.
            //Loops once every hour.
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(new TimeSpan(WaitHoursBeforeRefresh, 0, 0), stoppingToken);

                var dbTasks = await userTaskService.GetAllUserTasksAsync(stoppingToken);
                var newTasks = dbTasks.Where(t => !Reminders.Any(k => k.UserTask == t));


                foreach (var newTask in newTasks)
                    AddNewReminderData(this, newTask);

                foreach (var reminder in Reminders)
                    CreateReminderDataTimers(reminder, WaitHoursBeforeRefresh);
            }
        }

        /// <summary>
        /// Creates new <see cref="ReminderData"/> for the given service
        /// </summary>
        /// <param name="service">the service to add the reminder data to</param>
        /// <param name="userTask">the <see cref="UserTask"/> to create the <see cref="ReminderData"/> of</param>
        private static void AddNewReminderData(ReminderService service, UserTask userTask)
        {
            var reminder = new ReminderData(userTask);

            //invoke events for this service when the events in ReminderData are elapsed.
            reminder.RemindUserReached += (_, e) => service.ReminderTimeReached?.Invoke(service, e);
            reminder.DueDateReached += (_, e) => service.DueDateReached?.Invoke(service, e);
            service.Reminders.Add(reminder);

            //look for dates changed.
            userTask.PropertyChanged += (_, e) => UserTask_PropertyChanged(reminder, service.WaitHoursBeforeRefresh, e);
        }

        /// <summary>
        /// Creates the <see cref="ReminderData.RemindUserTimer"/> and <see cref="ReminderData.DueDateTimer"/> and
        /// starts them if less then 1 hour is left to reach the time of properties in <see cref="ReminderData.UserTask"/>
        /// </summary>
        /// <param name="sender">the <see cref="ReminderData"/> to set the timers of</param>
        private static void CreateReminderDataTimers(ReminderData sender, int refreshHour)
        {
            var task = sender.UserTask;
            var utcTime = DateTime.UtcNow;
            if (utcTime < task.RemindUserUtc)
            {
                var timeleft = task.RemindUserUtc - utcTime;

                //The service is checking for timer updates every hour anyway, so we do not need to start the timer early
                if (timeleft.Value.TotalHours < refreshHour)
                    ReminderData.SetRemindUserTimer(sender, timeleft.Value.Ticks);
            }
            if (utcTime < task.DueDateUtc)
            {
                var timeleft = task.DueDateUtc - utcTime;

                if (timeleft.Value.TotalHours < refreshHour)
                    ReminderData.SetDueDateTimer(sender, timeleft.Value.Ticks);
            }
        }

        /// <summary>
        /// Attempts to create data timer when the <see cref="UserTask.RemindUser"/> or <see cref="UserTask.DueDate"/> are changed.
        /// </summary>
        /// <param name="sender">the current <see cref="ReminderData"/> instance for this <see cref="UserTask"/></param>
        /// <param name="refreshHour">The wait hours set in the <see cref="ReminderService"/></param>
        private static void UserTask_PropertyChanged(ReminderData? sender, int refreshHour, PropertyChangedEventArgs e)
        {
            if (sender is not null && (e.PropertyName == nameof(UserTask.RemindUser) || e.PropertyName == nameof(UserTask.DueDate)))
                CreateReminderDataTimers(sender, refreshHour);
        }
    }
}
