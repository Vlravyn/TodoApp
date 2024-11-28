using MvvmEssentials.Core;
using System.Collections.ObjectModel;
using TodoApp.Core.EntityFramework;

namespace TodoApp.Core.DataModels
{
    /// <summary>
    /// Custom container for multiple <see cref="UserTask"/>
    /// </summary>
    public class TaskList : ObservableObject
    {
        private string _title;

        public TaskList()
        {
        }

        public Guid Id { get; set; }
        
        /// <summary>
        /// The title for this task list.
        /// </summary>
        public required string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// The <see cref="UserTask"/> that this task list contains
        /// </summary>
        public ObservableCollection<UserTask> Tasks { get; set; } = [];

        /// <summary>
        /// Used for database. Ignore this.
        /// </summary>
        internal List<UserTaskListsJoinedTable> UserTaskLists { get; set; } = [];

    }
}
