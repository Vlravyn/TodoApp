using Timer = System.Timers.Timer;

namespace TodoApp.Core.DataModels
{
    /// <summary>
    /// Wrapper for <see cref="UserTask"/> to set the timer for each <see cref="UserTask"/> and raise events individually.
    /// </summary>
    internal class ReminderData
    {
        /// <summary>
        /// Raises when the <see cref="RemindUserTimer"/> ends.
        /// </summary>
        public event EventHandler<ReminderEventArgs> RemindUserReached;

        /// <summary>
        /// Raises when the <see cref="DueDateTimer"/> ends.
        /// </summary>
        public event EventHandler<ReminderEventArgs> DueDateReached;

        /// <summary>
        /// The user task that is being stored for reminder.
        /// </summary>
        public UserTask UserTask { get; set; }


        /// <summary>
        /// Timer for <see cref="UserTask.RemindUser"/>
        /// </summary>
        public Timer? RemindUserTimer { get; set; }

        /// <summary>
        /// Timer for <see cref="UserTask.DueDate"/>
        /// </summary>
        public Timer? DueDateTimer { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="ReminderData"/>
        /// </summary>
        /// <param name="userTask"></param>
        public ReminderData(UserTask userTask)
        {
            UserTask = userTask;
        }

        /// <summary>
        /// Sets the <see cref="RemindUserTimer"/> to elapse when the time is reached.(Elapses once only).
        /// </summary>
        /// <param name="data">the <see cref="ReminderData"/> to set the timer of</param>
        /// <param name="ticks">The ticks left for the first elapse</param>
        internal static void SetRemindUserTimer(ReminderData data, long ticks)
        {
            data.RemindUserTimer = new Timer(new TimeSpan(ticks))
            {
                //run the timer only once.
                AutoReset = false
            };

            data.RemindUserTimer.Elapsed += (_, _) => data.RemindUserReached?.Invoke(data, new ReminderEventArgs(data.UserTask));
            data.RemindUserTimer.Start();
        }

        /// <summary>
        /// Sets the <see cref="DueDateTimer"/> to elapse when the time is reached.(Elapses once only).
        /// </summary>
        /// <param name="data">the <see cref="ReminderData"/> to set the timer of</param>
        /// <param name="ticks">The ticks left for the first elapse</param>
        internal static void SetDueDateTimer(ReminderData data, long ticks)
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
