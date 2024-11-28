using TodoApp.Core.DataModels;

namespace TodoApp.Core.EntityFramework
{
    public class UserTaskListsJoinedTable
    {
        public Guid UserTaskId { get; set; }
        public Guid TaskListId { get; set; }
        public UserTask UserTask { get; set; }
        public TaskList TaskList { get; set; }
    }
}
