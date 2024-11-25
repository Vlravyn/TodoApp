using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core.Navigation;
using System.Collections.ObjectModel;
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

        protected async override void OnChangesSaved(object? sender, SavedChangesEventArgs e)
        {
            base.OnChangesSaved(sender, e);
            await UpdateList();
        }

        /// <summary>
        /// Updates the <see cref="CompletedTasks"/>
        /// </summary>
        private async Task UpdateList()
        {
            CompletedTasks = new ObservableCollection<UserTask>(await userTaskService.GetCompletedUserTasksAsync());
            OnPropertyChanged(nameof(CompletedTasks));
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if(CompletedTasks is null ||  CompletedTasks.Count == 0)
                await UpdateList();
        }

        public void OnNavigatedFrom()
        {
        }

        #endregion Private Methods
    }
}