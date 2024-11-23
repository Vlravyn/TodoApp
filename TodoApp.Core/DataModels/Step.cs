using Microsoft.EntityFrameworkCore;
using MvvmEssentials.Core;

namespace TodoApp.Core.DataModels
{
    [Owned]
    public class Step : ObservableObject
    {
        private string _title;
        private bool _isCompleted;

        public Guid Id { get; set; }

        /// <summary>
        /// Title for this task.
        /// </summary>
        ///<exception cref="ArgumentException">thrown when user tries to set title to null/empty/whitespace.</exception>
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("title of the task cannot be null or empty");

                SetProperty(ref _title, value);
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }
    }
}