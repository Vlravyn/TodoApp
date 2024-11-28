using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.CustomControls
{
    public class TimePicker : Control
    {
        //Set to when the TimeProperty is changing using another callback to prevent looping.
        bool isTimeChangingFlag = false;

        //Set to when the HourProperty or MinuteProperty or MeridiumProperty is changing using another callback to prevent looping.
        bool isHourMinuteChangingFlag = false;

        /// <summary>
        /// Raises when the <see cref="Time"/> changes
        /// </summary>
        public event EventHandler TimeChanged;


        #region DependencyProperties

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(nameof(Hour), typeof(int), typeof(TimePicker), new PropertyMetadata(HourMinuteChangedCallback));
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(nameof(Minute), typeof(int), typeof(TimePicker), new PropertyMetadata(HourMinuteChangedCallback));
        public static readonly DependencyProperty MeridiumProperty = DependencyProperty.Register(nameof(Meridium), typeof(TimeMeridium), typeof(TimePicker), new PropertyMetadata(TimeMeridium.AM, HourMinuteChangedCallback));
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(), TimeChangedCallback));
        public static readonly DependencyProperty Is24HourFormatProperty = DependencyProperty.Register(nameof(Is24HourFormat), typeof(bool), typeof(TimePicker), new PropertyMetadata(false));

        #endregion

        #region Public Properties

        /// <summary>
        /// The hour value that the <see cref="Time"/> will have in this timepicker.
        /// Max and Min values are set in the default style.
        /// </summary>
        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        /// <summary>
        /// The minute value that the <see cref="Time"/> will have in this timepicker.
        /// Max and min value are set in the dafault style.
        /// </summary>
        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        /// <summary>
        /// The TimeMeridium that this <see cref="TimePicker"/> is using.
        /// Is not used when <see cref="Is24HourFormat"/> is true.
        /// </summary>
        public TimeMeridium Meridium
        {
            get => (TimeMeridium)GetValue(MeridiumProperty);
            set => SetValue(MeridiumProperty, value);
        }

        /// <summary>
        /// The time that is currently set in this <see cref="TimePicker"/>
        /// Bind to this property to get the time.
        /// </summary>
        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        /// <summary>
        /// Sets the <see cref="TimePicker"/> to use 12-hour or 24-hour format.
        /// Is used in Setting the maximum and minimum allowed values of <see cref="Hour"/> and <see cref="Minute"/> in the default style.
        /// Uses the system default if not set explicitly.
        /// </summary>
        public bool Is24HourFormat
        {
            get => (bool)GetValue(Is24HourFormatProperty);
            set => SetValue(Is24HourFormatProperty, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="TimePicker"/>
        /// </summary>
        public TimePicker()
        {
        }

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (string.IsNullOrEmpty(DateTimeFormatInfo.CurrentInfo.AMDesignator))
                Use24HourFormat();
        }

        private void Use24HourFormat()
        {
            Is24HourFormat = true;
        }

        #region Callbacks
        private static void HourMinuteChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TimePicker picker && !picker.isHourMinuteChangingFlag)
            {
                if(picker.Time.Hours != picker.Hour && picker.Time.Hours != picker.Hour + 12 || picker.Time.Minutes != picker.Minute)
                {
                    var hour = picker.Hour;

                    if (!picker.Is24HourFormat && picker.Meridium == TimeMeridium.PM)
                        hour += 12;

                    picker.isTimeChangingFlag = true;
                    picker.Time = new TimeSpan(hour, picker.Minute, 0);
                    picker.isTimeChangingFlag = false;
                }
                else if(!picker.Is24HourFormat && picker.Meridium == TimeMeridium.PM)
                {
                    var hour = picker.Hour;
                    hour += 12;

                    picker.isTimeChangingFlag = true;
                    picker.Time = new TimeSpan(hour, picker.Minute, 0);
                    picker.isTimeChangingFlag = false;
                }
            }
        }
        private static void TimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TimePicker picker && !picker.isTimeChangingFlag)
            {
                if (picker.Time == TimeSpan.Zero)
                    return;
                if(picker.Time.Hours != picker.Hour && picker.Time.Hours != picker.Hour + 12 || picker.Time.Minutes != picker.Minute)
                {
                    var hour = picker.Time.Hours;
                    if (hour > 12 && !picker.Is24HourFormat)
                    {
                        hour -= 12;
                        picker.Meridium = TimeMeridium.PM;
                    }

                    picker.isHourMinuteChangingFlag = true;
                    picker.Hour = hour;
                    picker.Minute = picker.Time.Minutes;
                    picker.isHourMinuteChangingFlag = false;
                }

                picker.TimeChanged?.Invoke(picker, EventArgs.Empty);
            }
        }
        
        #endregion Callbacks
    }
}