using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.CustomControls
{
    /// <summary>
    /// The Meridium for the time
    /// </summary>
    public enum TimeMeridium
    {
        /// <summary>
        /// Represents Ante Meridium time
        /// </summary>
        AM = 1,

        /// <summary>
        /// Represents Post Meridium time
        /// </summary>
        PM = 2
    }

    public class TimePicker : Control
    {
        public event EventHandler TimeChanged;

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(nameof(Hour), typeof(int), typeof(TimePicker), new PropertyMetadata());
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(nameof(Minute), typeof(int), typeof(TimePicker), new PropertyMetadata());
        public static readonly DependencyProperty MeridiumProperty = DependencyProperty.Register(nameof(Meridium), typeof(TimeMeridium), typeof(TimePicker), new PropertyMetadata(TimeMeridium.AM));
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan()));
        public static readonly DependencyProperty Is24HourFormatProperty = DependencyProperty.Register(nameof(Is24HourFormat), typeof(bool), typeof(TimePicker), new PropertyMetadata(false));

        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        public TimeMeridium Meridium
        {
            get => (TimeMeridium)GetValue(MeridiumProperty);
            set => SetValue(MeridiumProperty, value);
        }

        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public bool Is24HourFormat
        {
            get => (bool)GetValue(Is24HourFormatProperty);
            set => SetValue(Is24HourFormatProperty, value);
        }

        public TimePicker()
        {
            
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Hour = 12;
            Minute = 0;

            if (string.IsNullOrEmpty(DateTimeFormatInfo.CurrentInfo.AMDesignator))
                Use24HourFormat();
        }

        private void Use24HourFormat()
        {
            Is24HourFormat = true;
        }

        #region Callbacks

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if(Is24HourFormat)
                Time = new(Hour, Minute, 0);
            else
            {
                if (Meridium == TimeMeridium.AM)
                    Time = new TimeSpan(Hour, Minute, 0);
                else if(Meridium == TimeMeridium.PM)
                    Time = new TimeSpan(Hour + 12, Minute, 0);
            }
        }
        #endregion Callbacks
    }
}