using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TodoApp.Core.DataModels;

namespace TodoApp.Core.Services
{
    /// <summary>
    /// Contract for service that stores/updates the user tasks.
    /// </summary>
    public interface IUserTaskService
    {
        event EventHandler<AddingNewEventArgs> UserTaskAdded;
        event EventHandler<AddingNewEventArgs> UserTaskDeleted;
        event EventHandler<AddingNewEventArgs> UserTaskUpdated;
        event EventHandler<AddingNewEventArgs> TaskListAdded;
        event EventHandler<AddingNewEventArgs> TaskListDeleted;
        event EventHandler<AddingNewEventArgs> TaskListUpdated;
        event EventHandler<SavedChangesEventArgs> SavedChanges;

        /// <summary>
        /// Adds a new user task to save
        /// </summary>
        /// <param name="userTask">the user </param>
        /// <returns>a new tracked instance of the user task</returns>
        Task<UserTask> AddAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Addds a new <see cref="TaskList"/> to the database.
        /// </summary>
        /// <param name="list">an instance of the new list to add</param>
        /// <param name="token">the cancellation token</param>
        /// <returns>the tracked version of the task list connected to the database</returns>
        Task<TaskList> AddAsync(TaskList list, CancellationToken token = default);

        /// <summary>
        /// Updates an existing <see cref="UserTask"/>
        /// </summary>
        /// <param name="userTask">the updated instance of <see cref="TaskList"/></param>
        /// <returns><see langword="true"/> if the operation was successful</returns>
        Task<bool> UpdateAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Updates an existing <see cref="TaskList"/>
        /// </summary>
        /// <param name="list">the updated instance of the <see cref="TaskList"/></param>
        /// <param name="token">the cancellation token</param>
        /// <returns><see langword="true"/> if the operation was successful</returns>
        public Task<bool> UpdateAsync(TaskList list, CancellationToken token = default);

        /// <summary>
        /// Deletes a <see cref="UserTask"/>
        /// </summary>
        /// <param name="userTask">the user task to delete</param>
        /// <returns><see langword="true"/> if the operation was successful</returns>
        Task<bool> DeleteAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Deletes a <see cref="TaskList"/>
        /// </summary>
        /// <param name="list">the <see cref="TaskList"/> to delete</param>
        /// <returns><see langword="true"/> if the operation was successful</returns>
        Task<bool> DeleteAsync(TaskList list, CancellationToken token = default);

        /// <summary>
        /// Saves the changes made to the dbContext
        /// </summary>
        /// <param name="token">the cancellation token</param>
        /// <returns><see langword="true"/> if the changes were made successfully</returns>
        Task<bool> SaveChangesAsync(CancellationToken token = default);

        /// <summary>
        /// Attempts to get a user take by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns>collection of <see cref="UserTask"/></returns>
        Task<IEnumerable<UserTask>?> GetUserTaskByTitleAsync(string title, CancellationToken token = default);

        /// <summary>
        /// Gets all the user tasks
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserTask>> GetAllUserTasksAsync(CancellationToken token = default);

        /// <summary>
        /// Gets all the <see cref="TaskList"/> stored in the database.
        /// </summary>
        /// <param name="token">the cancellation token</param>
        /// <returns>collection of <see cref="TaskList"/></returns>
        Task<IEnumerable<TaskList>> GetAllTaskListsAsync(CancellationToken token = default);

        /// <summary>
        /// Gets all the completed user tasks.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserTask>> GetCompletedUserTasksAsync(CancellationToken token = default);
    }
}