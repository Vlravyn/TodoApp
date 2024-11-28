using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using System.ComponentModel;
using TodoApp.Core.DataModels;
using TodoApp.Core.Services;

namespace TodoApp.ViewModels
{
    public class TaskDetailViewModel : ObservableObject, INavigationAware
    {
        private UserTask _userTask;
        private readonly IUserTaskService _userTaskService;
        private readonly INavigationService _navigationService;

        public UserTask Task
        {
            get => _userTask;
            set => SetProperty(ref _userTask, value);
        }

        public RelayCommand CloseCommand => new(Close);
        public RelayCommand AddNewStepCommand => new(AddNewStep);
        public RelayCommand<Step> ToggleCompletedCommand => new(ToggleCompleted);
        public RelayCommandAsync<Step> DeleteStepCommand => new(DeleteStep);
        public RelayCommandAsync<Step> PromoteToTaskCommand => new(PromoteToTask);

        /// <summary>
        /// Creates an instance of <see cref="TaskDetailViewModel"/>
        /// </summary>
        public TaskDetailViewModel(IUserTaskService userTaskService, INavigationService navigationService)
        {
            _userTaskService = userTaskService;
            _navigationService = navigationService;
        }

        protected void OnUserTaskDeleted(object? sender, AddingNewEventArgs e)
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

            _userTaskService.UpdateAsync(Task);
        }


        /// <summary>
        /// Closes the view
        /// </summary>
        private void Close()
        {
            _userTaskService.SaveChangesAsync();
            _navigationService.Navigate("TaskDetailRegion", ViewType.None, new NavigationParameters());
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

            var result = await _userTaskService.UpdateAsync(Task, token);

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

            await _userTaskService.AddAsync(new UserTask()
            {
                IsCompleted = step.IsCompleted,
                Title = step.Title
            }, token);
            Task.Steps.Remove(step);
        }

        public void OnNavigatedFrom()
        {
            Task = null;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var pair = parameters.FirstOrDefault(t => t.Key == nameof(UserTask));

            if (pair.Equals(default(KeyValuePair<string, object>)))
                throw new ArgumentException($"The passed in parameter does not contain a {nameof(KeyValuePair)} with key {nameof(UserTask)} in {nameof(TaskDetailViewModel)}");

            if (pair.Value is not UserTask)
                throw new ArgumentException($"Unknown type was passed as parameter to {nameof(TaskDetailViewModel)} with key {nameof(UserTask)}");

            Task = (UserTask)pair.Value;
        }
    }
}