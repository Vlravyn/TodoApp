using TodoApp.Core.DataModels;

namespace TodoApp.Reminder
{
    public class ReminderEventArgs : EventArgs
    {
        public UserTask UserTask { get; set; }
        public ReminderEventArgs(UserTask task)
        {
            UserTask = task;
        }
    }
}
