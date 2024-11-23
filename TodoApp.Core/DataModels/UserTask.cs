using MvvmEssentials.Core;
using System.Collections.ObjectModel;

namespace TodoApp.Core.DataModels
{
    public class UserTask : ObservableObject
    {
        #region Private Members

        private string _title;
        private bool _isImportant;
        private bool _isCompleted;
        private DateTime _createdAtUtc;
        private string? _description;

        #endregion Private Members

        public UserTask()
        {
            Id = Guid.NewGuid();
            CreatedAtUtc = DateTime.UtcNow;
        }

        internal Guid Id { get; set; }

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

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

        public bool IsImportant
        {
            get => _isImportant;
            set => SetProperty(ref _isImportant, value);
        }

        public ObservableCollection<Step> Steps { get; set; } = new();

        /// <summary>
        /// CreatedAt property local time
        /// </summary>
        public DateTime CreatedAt
        {
            get => CreatedAtUtc.ToLocalTime();
        }

        public DateTime CreatedAtUtc
        {
            get => _createdAtUtc;
            init => _createdAtUtc = value;
        }

        /// <summary>
        /// Changeable DueDate for data binding.
        /// </summary>
        /// <exception cref="ArgumentException">thrown when the user tries to set the due date in the past.</exception>
        public DateTime? DueDate
        {
            get => DueDateUtc?.ToLocalTime();
            set
            {
                DueDateUtc = value?.ToUniversalTime();
                OnPropertyChanged(nameof(DueDate));
            }
        }

        /// <summary>
        /// Changeable RemindUser for DataBinding
        /// </summary>
        /// <exception cref="ArgumentException">thrown an exception when user tries to set reminder DateTime in the past.</exception>
        public DateTime? RemindUser
        {
            get => RemindUserUtc?.ToLocalTime();
            set
            {
                RemindUserUtc = value?.ToUniversalTime();
                OnPropertyChanged(nameof(RemindUser));
            }
        }

        /// <summary>
        /// DueDate for DbContext that stores the actual reminder DateTime in UTC
        /// </summary>
        public DateTime? DueDateUtc { get; internal set; }

        /// <summary>
        /// RemindUser for DbContext that stores the actual reminder DateTime in UTC
        /// </summary>
        public DateTime? RemindUserUtc { get; internal set; }
    }
}