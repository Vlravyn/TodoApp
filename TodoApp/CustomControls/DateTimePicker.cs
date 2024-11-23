using System.Windows;
using System.Windows.Controls;

namespace TodoApp.CustomControls
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(object))]
    public class DateTimePicker : Control
    {
        #region Events

        public event EventHandler SelectedDateTimeChanged;
        public event RoutedEventHandler PickerOpened;
        public event RoutedEventHandler PickerClosed;

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateTimePicker));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false, new PropertyChangedCallback(IsOpenChanged)));
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(DateOrTimeChangedCallback)));
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(DateOrTimeChangedCallback)));
        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(nameof(SelectedDateTime), typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(SelectedDateTimeChangedCallback)));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(DateTimePicker), new PropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(DateTimePicker), new PropertyMetadata());
        public static readonly DependencyProperty IsDateTimeExplicitlySetProperty = DependencyProperty.Register(nameof(IsDateTimeExplicitlySet), typeof(bool), typeof(DateTimePicker), new PropertyMetadata());
        #endregion

        #region Public Properties

        public bool IsDateTimeExplicitlySet
        {
            get => (bool)GetValue(IsDateTimeExplicitlySetProperty);
            private set => SetValue(IsDateTimeExplicitlySetProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// the selected date.
        /// </summary>
        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        /// <summary>
        /// The selected time
        /// </summary>
        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        /// <summary>
        /// THe DateTime that is set in this control
        /// </summary>
        public DateTime SelectedDateTime
        {
            get => (DateTime)GetValue(SelectedDateTimeProperty);
            set => SetValue(SelectedDateTimeProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        #endregion

        #region Constructors

        public DateTimePicker()
        {
        }

        #endregion

        #region Private Methods

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker p)
            {
                if (p.IsOpen)
                    p.PickerOpened?.Invoke(p, new RoutedEventArgs());
                else
                    p.PickerClosed?.Invoke(p, new RoutedEventArgs());
            }
        }

        private static void DateOrTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker p && (p.SelectedDateTime.Date != p.Date && p.Time != p.SelectedDateTime.TimeOfDay))
            {
                if(e.NewValue is DateTime newDate && newDate != DateTime.MinValue)
                    p.SelectedDateTime = p.Date;
                if(e.NewValue is TimeSpan newTime)
                    p.SelectedDateTime += newTime;
                p.SelectedDateTimeChanged?.Invoke(p, EventArgs.Empty);
            }
        }

        private static void SelectedDateTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker p && (p.SelectedDateTime.Date != p.Date && p.Time != p.SelectedDateTime.TimeOfDay))
            {
                p.Time = p.SelectedDateTime.TimeOfDay;
                p.Date = p.SelectedDateTime.Date;
                p.SelectedDateTimeChanged?.Invoke(p, EventArgs.Empty);
                if (!p.IsDateTimeExplicitlySet)
                    p.IsDateTimeExplicitlySet = true;
            }
        }

        #endregion
    }
}