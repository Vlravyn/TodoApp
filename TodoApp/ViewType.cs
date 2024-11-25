using MvvmEssentials.Core;
using MvvmEssentials.Core.Navigation;
using System.Runtime.Serialization;
using TodoApp.Views;

namespace TodoApp
{
    /// <summary>
    /// Types of pages that can be used by the application.
    /// </summary>
    [IsNavigationContentEnum]
    internal enum ViewType
    {
        [NavigateTo()]
        None,

        /// <summary>
        /// Refers to the page that shows all the tasks
        /// </summary>
        [EnumMember(Value = "allTasks")]
        [NavigateTo(DestinationType = typeof(AllTasksPage))]
        AllTasks,

        /// <summary>
        /// refers to the page which shows all the completed tasks
        /// </summary>
        [NavigateTo(DestinationType = typeof(CompletedTasksPage))]
        [EnumMember(Value = "completedTasks")]
        CompletedTasks,

        /// <summary>
        /// refers to the page that shows the details of a task
        /// </summary>
        [NavigateTo(DestinationType = typeof(TaskDetailPage))]
        [EnumMember(Value = "taskDetails")]
        TaskDetails
    }
}