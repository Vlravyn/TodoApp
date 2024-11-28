using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class CompletedTasksViewModel : TaskCollectionViewModelBase, INavigationAware
    {
        #region Public Properties

        public ObservableCollection<UserTask> CompletedTasks { get; set; } = new();

        #endregion Public Properties

        #region Constructors

        public CompletedTasksViewModel(IUserTaskService userTaskService, INavigationService navigationService)
            : base(userTaskService, navigationService)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnUserTaskUpdated(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskUpdated(sender, e);
            if (e.NewObject is UserTask task)
            {
                if (CompletedTasks.Contains(task))
                {
                    if (task.IsCompleted is false)
                        CompletedTasks.Remove(task);
                }
                else
                {
                    if (task.IsCompleted)
                        CompletedTasks.Add(task);
                }
            }
        }
        protected override void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
        {
            base.OnUserTaskDeleted(sender, e);
            if (CompletedTasks.Contains(e.NewObject))
                CompletedTasks.Remove(((UserTask)e.NewObject));
        }
        protected override void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            base.OnChangesSaved(sender, e);
            OnPropertyChanged(nameof(CompletedTasks));
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if(CompletedTasks is null ||  CompletedTasks.Count == 0)
            {
                CompletedTasks = new ObservableCollection<UserTask>(await _userTaskService.GetCompletedUserTasksAsync());
                OnPropertyChanged(nameof(CompletedTasks));
            }
        }

        public void OnNavigatedFrom()
        {
        }

        #endregion Private Methods
    }
}