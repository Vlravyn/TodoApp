using TodoApp.Core.DataModels;

namespace TodoApp.Core.EntityFramework
{
    /// <summary>
    /// Entity for Many-To-Many Relationship between <see cref="UserTask"/> and <see cref="TaskList"/>
    /// </summary>
    public class UserTaskListsJoinedTable
    {
        public Guid UserTaskId { get; set; }
        public Guid TaskListId { get; set; }
        public UserTask UserTask { get; set; }
        public TaskList TaskList { get; set; }
    }
}
