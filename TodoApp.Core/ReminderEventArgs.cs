using TodoApp.Core.DataModels;

namespace TodoApp.Core
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
