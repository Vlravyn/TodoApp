using Timer = System.Timers.Timer;

namespace TodoApp.Core.DataModels
{
    internal class ReminderData
    {
        public event EventHandler<ReminderEventArgs> RemindUserReached;
        public event EventHandler<ReminderEventArgs> DueDateReached;
        public UserTask UserTask { get; set; }

        public ReminderData(UserTask userTask)
        {
            UserTask = userTask;
        }

        public Timer? RemindUserTimer { get; set; }
        public System.Timers.Timer? DueDateTimer { get; set; }

        public static void SetRemindUserTimer(ReminderData data, long ticks)
        {
            data.RemindUserTimer = new Timer(new TimeSpan(ticks))
            {
                //run the timer only once.
                AutoReset = false
            };

            data.RemindUserTimer.Elapsed += (_, _) => data.RemindUserReached?.Invoke(data, new ReminderEventArgs(data.UserTask));
            data.RemindUserTimer.Start();
        }
        public static void SetDueDateTimer(ReminderData data, long ticks)
        {
            data.DueDateTimer = new Timer(new TimeSpan(ticks))
            {
                //run the timer only once.
                AutoReset = false
            };

            data.DueDateTimer.Elapsed += (_, _) => data.DueDateReached?.Invoke(data, new ReminderEventArgs(data.UserTask));
            data.DueDateTimer.Start();
        }
    }
}
