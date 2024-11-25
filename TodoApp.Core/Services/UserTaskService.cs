using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.EntityFramework;

namespace TodoApp.Core.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly UserTasksDbContext dbContext;

        public event EventHandler<AddingNewEventArgs> UserTaskAdded;
        public event EventHandler<AddingNewEventArgs> UserTaskDeleted;
        public event EventHandler<AddingNewEventArgs> UserTaskUpdated;
        public event EventHandler<SavedChangesEventArgs> UserTaskSavedChanges;

        public UserTaskService(UserTasksDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.SavedChanges += (_, e) => UserTaskSavedChanges?.Invoke(this, e);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <exception cref="ArgumentNullException">thrown user task is null</exception>
        public async Task<UserTask> AddAsync(UserTask userTask, CancellationToken token = default)
        {
            if (userTask == null)
                throw new ArgumentNullException("cannot add null to the database");

            await dbContext.Database.EnsureCreatedAsync(token);
            var tracked = await dbContext.UserTasks.AddAsync(userTask, token);
            await SaveChangesAsync(token);
            UserTaskAdded?.Invoke(this, new AddingNewEventArgs(tracked.Entity));
            return tracked.Entity;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken token)
        {
            return await dbContext.SaveChangesAsync(token) != 0;
        }

        public async Task<bool> DeleteAsync(UserTask userTask, CancellationToken token = default)
        {
            dbContext.UserTasks.Remove(userTask);
            await dbContext.SaveChangesAsync(token);
            UserTaskDeleted?.Invoke(this, new AddingNewEventArgs(userTask));
            return true;
        }

        public async Task<IEnumerable<UserTask>> GetAllUserTasksAsync(CancellationToken token = default)
        {
            await dbContext.Database.EnsureCreatedAsync(token);
            return await dbContext.UserTasks.AsTracking().ToListAsync(token);
        }

        public async Task<IEnumerable<UserTask>?> GetByTitleAsync(string title, CancellationToken token = default)
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

        public async Task<IEnumerable<UserTask>> GetCompletedUserTasksAsync(CancellationToken token = default)
        {
            return await dbContext.UserTasks.Where(t => t.IsCompleted).AsTracking().ToListAsync(token);
        }
    }
}