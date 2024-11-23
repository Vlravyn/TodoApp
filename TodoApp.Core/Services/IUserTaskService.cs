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

        /// <summary>
        /// Adds a new user task to save
        /// </summary>
        /// <param name="userTask">the user </param>
        /// <returns>a new tracked instance of the user task</returns>
        Task<UserTask> AddAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Updates an existing user task
        /// </summary>
        /// <param name="userTask">the updated user task</param>
        /// <returns>true if the operation was successful</returns>
        Task<bool> UpdateAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Deletes a user task
        /// </summary>
        /// <param name="userTask">the user task to delete</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(UserTask userTask, CancellationToken token = default);

        /// <summary>
        /// Saves the changes made to the dbContext
        /// </summary>
        /// <param name="token">the cancellation token</param>
        /// <returns>true if the changes were made successfully</returns>
        Task<bool> SaveChangesAsync(CancellationToken token = default);

        /// <summary>
        /// Attempts to get a user take by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<IEnumerable<UserTask>?> GetByTitleAsync(string title, CancellationToken token = default);

        /// <summary>
        /// Gets all the user tasks
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserTask>> GetAllUserTasksAsync(CancellationToken token = default);

        /// <summary>
        /// Gets all the completed user tasks.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserTask>> GetCompletedUserTasksAsync(CancellationToken token = default);
    }
}