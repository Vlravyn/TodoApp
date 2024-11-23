using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.WPF.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskDetailViewModel : ObservableObject, INavigationAware
    {
        #region Private members

        private UserTask _userTask;

        private readonly IUserTaskService userTaskService;
        private readonly INavigationService navigationService;

        #endregion Private members

        #region Public properties

        public UserTask Task
        {
            get => _userTask;
            set => SetProperty(ref _userTask, value);
        }

        #endregion Public properties

        #region RelayCommands

        public RelayCommand CloseCommand => new(Close);
        public RelayCommand AddNewStepCommand => new(AddNewStep);
        public RelayCommand<Step> ToggleCompletedCommand => new(ToggleCompleted);
        public RelayCommandAsync<Step> DeleteStepCommand => new(DeleteStep);
        public RelayCommandAsync<Step> PromoteToTaskCommand => new(PromoteToTask);

        #endregion RelayCommands

        #region Constructors

        public TaskDetailViewModel(IUserTaskService userTaskService, INavigationService navigationService)
        {
            this.userTaskService = userTaskService;
            this.navigationService = navigationService;

            
            this.userTaskService.UserTaskDeleted += TaskDeleted;
        }

        #endregion Constructors

        #region Private Methods

        private void TaskDeleted(object? sender, System.ComponentModel.AddingNewEventArgs e)
        {
            if (e.NewObject == Task)
            {
                Task = null;
                Close();
            }
        }

        /// <summary>
        /// adds a new step to the <see cref="Task"/>
        /// </summary>
        /// <param name="t"></param>
        private void AddNewStep()
        {
            Task.Steps.Add(new Step()
            {
                IsCompleted = false,
                Title = $"Step {Task.Steps.Count + 1}"
            });

            userTaskService.UpdateAsync(Task);
        }


        /// <summary>
        /// Closes the view
        /// </summary>
        private void Close()
        {
            userTaskService.SaveChangesAsync();
            navigationService.Navigate("TaskDetailRegion", ViewType.None, new NavigationParameters());
        }

        /// <summary>
        /// Toggles the <see cref="Step.IsCompleted"/> property
        /// </summary>
        /// <param name="step">the step the toggle the property of</param>
        private void ToggleCompleted(Step? step)
        {
            if (step != null)
                step.IsCompleted = !step.IsCompleted;
        }

        /// <summary>
        /// Deletes the step from the database
        /// </summary>
        /// <param name="step">the step to delete</param>
        /// <param name="token">the the cancellation token</param>
        /// <returns>the task</returns>
        private async Task DeleteStep(Step? step, CancellationToken token = default)
        {
            if (step != null)
                Task.Steps.Remove(step);

            var result = await userTaskService.UpdateAsync(Task, token);

            if (result is false)
                Task.Steps.Add(step);
        }

        /// <summary>
        /// Promotes a <see cref="Step"/> to <see cref="UserTask"/>.
        /// </summary>
        /// <param name="step">the step to promote</param>
        /// <param name="token">the cancellation token</param>
        /// <returns>the task</returns>
        private async Task PromoteToTask(Step? step, CancellationToken token = default)
        {
            if (step == null)
                return;

            await userTaskService.AddAsync(new UserTask()
            {
                IsCompleted = step.IsCompleted,
                Title = step.Title
            }, token);
            Task.Steps.Remove(step);
        }

        #endregion Private Methods

        #region INavigationAware

        public void OnNavigatedFrom()
        {
            Task = null;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Task = parameters.FirstOrDefault(t => t.Key == nameof(UserTask)).Value as UserTask;
        }

        #endregion INavigationAware
    }
}