using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.EntityFramework;

namespace TodoApp.Core.Services
{
    /// <summary>
    /// Implementation of <see cref="IUserTaskService"/>
    /// </summary>
    public class UserTaskService : IUserTaskService
    {
        private readonly UserTasksDbContext dbContext;

        public event EventHandler<AddingNewEventArgs> UserTaskAdded;
        public event EventHandler<AddingNewEventArgs> UserTaskDeleted;
        public event EventHandler<AddingNewEventArgs> UserTaskUpdated;
        public event EventHandler<SavedChangesEventArgs> SavedChanges;
        public event EventHandler<AddingNewEventArgs> TaskListAdded;
        public event EventHandler<AddingNewEventArgs> TaskListDeleted;
        public event EventHandler<AddingNewEventArgs> TaskListUpdated;

        /// <summary>
        /// Creates an instance of <see cref="UserTaskService"/>
        /// </summary>
        /// <param name="dbContext">the <see cref="DbContext"/> to use for this service</param>
        public UserTaskService(UserTasksDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.SavedChanges += (_, e) => SavedChanges?.Invoke(this, e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <exception cref="ArgumentNullException">thrown user task is <see langword="null"/></exception>
        public async Task<UserTask> AddAsync(UserTask userTask, CancellationToken token = default)
        {
            if (userTask == null)
                throw new ArgumentNullException(nameof(userTask), "cannot add null to the database");

            await dbContext.Database.EnsureCreatedAsync(token);
            var tracked = await dbContext.UserTasks.AddAsync(userTask, token);
            await SaveChangesAsync(token);
            UserTaskAdded?.Invoke(this, new AddingNewEventArgs(tracked.Entity));
            return tracked.Entity;
        }

        public async Task<TaskList> AddAsync(TaskList list, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(nameof(list));
            await dbContext.Database.EnsureCreatedAsync(token);
            var tracked = await dbContext.TaskLists.AddAsync(list, token);
            await SaveChangesAsync(token);
            TaskListAdded?.Invoke(this, new AddingNewEventArgs(tracked.Entity));
            return tracked.Entity;
        }
        public async Task<bool> SaveChangesAsync(CancellationToken token)
        {
            var numberOfChangesSaved = await dbContext.SaveChangesAsync(token);
            var result = numberOfChangesSaved != 0;

            if (result)
                SavedChanges?.Invoke(this, new SavedChangesEventArgs(true, numberOfChangesSaved));
            return result;
        }

        public async Task<bool> DeleteAsync(UserTask userTask, CancellationToken token = default)
        {
            dbContext.UserTasks.Remove(userTask);
            var result = await dbContext.SaveChangesAsync(token);

            if (result == 0)
                return false;

            UserTaskDeleted?.Invoke(this, new AddingNewEventArgs(userTask));
            return true;
        }
        public async Task<bool> DeleteAsync(TaskList list, CancellationToken token = default)
        {
            dbContext.TaskLists.Remove(list);
            var result = await dbContext.SaveChangesAsync(token);

            if (result == 0)
                return false;

            TaskListDeleted?.Invoke(this, new AddingNewEventArgs(list));
            return true;
        }

        public async Task<IEnumerable<UserTask>> GetAllUserTasksAsync(CancellationToken token = default)
        {
            await dbContext.Database.EnsureCreatedAsync(token);
            return await dbContext.UserTasks.Include(t => t.UserTaskLists).AsTracking().ToListAsync(token);
        }

        public async Task<IEnumerable<TaskList>> GetAllTaskListsAsync(CancellationToken token = default)
        {
            await dbContext.Database.EnsureCreatedAsync(token);
            return await dbContext.TaskLists.Include(t => t.UserTaskLists).AsTracking().ToListAsync(token);
        }


        public async Task<IEnumerable<UserTask>?> GetUserTaskByTitleAsync(string title, CancellationToken token = default)
        {
            return (await GetAllUserTasksAsync(token)).Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> UpdateAsync(UserTask userTask, CancellationToken token = default)
        {
            await dbContext.Database.EnsureCreatedAsync(token);
            dbContext.UserTasks.Update(userTask);
            await SaveChangesAsync(token);
            UserTaskUpdated?.Invoke(this, new AddingNewEventArgs(userTask));
            return true;
        }
        public async Task<bool> UpdateAsync(TaskList list, CancellationToken token = default)
        {
            await dbContext.Database.EnsureCreatedAsync(token);
            dbContext.TaskLists.Update(list);

            await SaveChangesAsync(token);
            TaskListUpdated?.Invoke(this, new AddingNewEventArgs(list));
            return true;
        }
        public async Task<IEnumerable<UserTask>> GetCompletedUserTasksAsync(CancellationToken token = default)
        {
            return await dbContext.UserTasks.Include(t => t.UserTaskLists).Where(t => t.IsCompleted).AsTracking().ToListAsync(token);
        }
    }
}