using MvvmEssentials.Core;
using System.Collections.ObjectModel;
using TodoApp.Core.EntityFramework;

namespace TodoApp.Core.DataModels
{
    public class TaskList : ObservableObject
    {
        private string _title;

        public TaskList()
        {
        }

        public Guid Id { get; set; }
        
        public required string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<UserTask> Tasks { get; set; } = new();
        internal List<UserTaskListsJoinedTable> UserTaskLists { get; set; } = new();

    }
}
