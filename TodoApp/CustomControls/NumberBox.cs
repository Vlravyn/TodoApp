using MvvmEssentials.Core.Commands;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace TodoApp.CustomControls
{
    public partial class NumberBox : CustomTextBox
    {
        public event EventHandler<DataTransferEventArgs> NumberChanged;

        #region DependencyProperties

        public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register(nameof(MaximumValue), typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue, MaxValueChangedCallback));
        public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register(nameof(MinimumValue), typeof(double), typeof(NumberBox), new PropertyMetadata(double.MinValue, MinValueChangedCallback));
        public static readonly DependencyProperty AllowDecimalProperty = DependencyProperty.Register(nameof(AllowDecimal), typeof(bool), typeof(NumberBox), new PropertyMetadata(true, new PropertyChangedCallback(AllowDecimalChangedCallback)));
        public static readonly DependencyProperty NumberRangeProperty = DependencyProperty.Register(nameof(NumberRange), typeof(NumberBoxRange), typeof(NumberBox), new PropertyMetadata(NumberBoxRange.AllowBoth, new PropertyChangedCallback(NumberRangeChangedCallback)));
        public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(nameof(MaxDecimalPlaces), typeof(ushort?), typeof(NumberBox), new PropertyMetadata(new PropertyChangedCallback(MaxDecimalPlacesChangedCallback)));
        public static readonly DependencyProperty ChangeByProperty = DependencyProperty.Register(nameof(ChangeBy), typeof(double), typeof(NumberBox), new PropertyMetadata());
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(nameof(Number), typeof(double?), typeof(NumberBox), new PropertyMetadata(0d, NumberChangedCallback));
        public static readonly DependencyProperty ChangeByButtonVisibilityProperty = DependencyProperty.Register(nameof(ChangeByButtonVisibility), typeof(Visibility), typeof(NumberBox), new PropertyMetadata(Visibility.Visible));
        #endregion DependencyProperties

        #region Public Properties

        public ChangeByOutOfBoundsBehaviours ChangeByOutOfBoundsBehaviour { get; set; }

        /// <summary>
        /// The number that is stored in this number box
        /// </summary>
        public double? Number
        {
            get => (double?)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }

        /// <summary>
        /// the highest number that is allowed by this number box
        /// </summary>
        public double MaximumValue
        {
            get => (double)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public NumberBoxRange NumberRange
        {
            get => (NumberBoxRange)GetValue(NumberRangeProperty);
            set => SetValue(NumberRangeProperty, value);
        }

        /// <summary>
        /// the lowest number that is allowed by this number box
        /// </summary>
        public double MinimumValue
        {
            get => (double)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        /// <summary>
        /// Allows the decimal to be used
        /// </summary>
        public bool AllowDecimal
        {
            get => (bool)GetValue(AllowDecimalProperty);
            set => SetValue(AllowDecimalProperty, value);
        }

        /// <summary>
        /// Specifies the max number of decimal places allowed.
        /// </summary>
        public ushort? MaxDecimalPlaces
        {
            get
            {
                if (AllowDecimal is false)
                    return 0;

                return (ushort)GetValue(MaxDecimalPlacesProperty);
            }
            set
            {
                if (AllowDecimal)
                    SetValue(MaxDecimalPlacesProperty, value);
            }
        }

        /// <summary>
        /// The regex that is being used by this number box
        /// </summary>
        public new Regex Regex
        {
            get => (Regex)GetValue(RegexProperty);
            private set => SetValue(RegexProperty, value);
        }

        /// <summary>
        /// Specifies the value to increase or decrease the number by when <see cref="IncreaseByCommand"/> and <see cref="DecreaseByCommand"/> are executed
        /// </summary>
        public double ChangeBy
        {
            get => (double)GetValue(ChangeByProperty);
            set => SetValue(ChangeByProperty, value);
        }

        /// <summary>
        /// Specifies the visibility of the buttons which execute the commands <see cref="IncreaseByCommand"/> and <see cref="DecreaseByCommand"/>
        /// </summary>
        public Visibility ChangeByButtonVisibility
        {
            get => (Visibility)GetValue(ChangeByButtonVisibilityProperty);
            set => SetValue(ChangeByButtonVisibilityProperty, value);
        }

        #endregion Public Properties

        #region RelayCommand

        public RelayCommand IncreaseByCommand => new(() => ChangeNumber(true));
        public RelayCommand DecreaseByCommand => new(() => ChangeNumber(false));

        #endregion RelayCommand

        #region Constructors

        /// <summary>
        /// Creates in instance of <see cref="NumberBox"/>
        /// </summary>
        public NumberBox()
        {
            UpdateRegex(this);

            SetBinding(TextProperty, new Binding(nameof(Number))
            {
                Mode = BindingMode.TwoWay,
                RelativeSource = RelativeSource.Self,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
            });
        }
        #endregion Constructors

        #region Private methods

        private void ChangeNumber(bool isIncreasing)
        {
            if (isIncreasing)
            {
                if(Number == MaximumValue)
                {
                    switch (ChangeByOutOfBoundsBehaviour)
                    {
                        case ChangeByOutOfBoundsBehaviours.LoopToOtherBound:
                            Number = MinimumValue;
                            break;
                    }
                    return;
                }

                Number += ChangeBy;
            }
            else
            {
                if(Number == MinimumValue)
                {
                    switch (ChangeByOutOfBoundsBehaviour)
                    {
                        case ChangeByOutOfBoundsBehaviours.LoopToOtherBound:
                            Number = MaximumValue;
                            break;
                    }
                    return;
                }

                Number -= ChangeBy;
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            var text = Text.Insert(CaretIndex, e.Text);

            if (!Regex.IsMatch(text))
            {
                e.Handled = true;
                return;
            }

            //just put the value if the first input is a negative sign
            if (text == "-")
                return;

            double number = double.Parse(text);

            if (number < MinimumValue || number > MaximumValue)
                e.Handled = true;

            if (AllowDecimal && MaxDecimalPlaces != 0 && GetDecimalPlaces(number) > MaxDecimalPlaces)
                e.Handled = true;
        }

        /// <summary>
        /// Sets the <see cref="Regex"/> according to the number box settings
        /// </summary>
        /// <param name="nb">the number box whose regex to set</param>
        private static void UpdateRegex(NumberBox nb)
        {
            switch (nb.NumberRange)
            {
                case NumberBoxRange.AllowBoth:
                    if (nb.AllowDecimal)
                        nb.Regex = PositiveNegatieDecimalNumber();
                    else
                        nb.Regex = PositiveNegativeInteger();
                    break;

                case NumberBoxRange.OnlyPositive:
                    if (nb.AllowDecimal)
                        nb.Regex = PositiveDecimalNumber();
                    else
                        nb.Regex = PositiveInteger();
                    break;

                case NumberBoxRange.OnlyNegative:
                    if (nb.AllowDecimal)
                        nb.Regex = NegativeDecimalNumber();
                    else
                        nb.Regex = NegativeInteger();
                    break;
            }
        }

        /// <summary>
        /// Gets the count of digits after the decimal for the number
        /// </summary>
        /// <param name="number">the number to count the decimal places of</param>
        /// <returns>the number</returns>
        private static byte GetDecimalPlaces(double number)
        {
            return BitConverter.GetBytes(decimal.GetBits((decimal)number)[3])[2];
        }

        private static void NumberChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
                nb.NumberChanged?.Invoke(nb, null);
        }

        private static void MaxDecimalPlacesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                if (nb.Number is null)
                    return;

                byte decimalPlaces = GetDecimalPlaces((double)nb.Number);

                //Removing the extra numbers that were entered after the decimal
                if (nb.MaxDecimalPlaces is not null && decimalPlaces > nb.MaxDecimalPlaces)
                    nb.Text = Math.Round((double)nb.Number, decimalPlaces).ToString();
            }
        }

        private static void NumberRangeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                UpdateRegex(nb);
                switch (nb.NumberRange)
                {
                    case NumberBoxRange.OnlyPositive:
                        if(nb.MinimumValue < 0)
                            nb.MinimumValue = 0;

                        if (nb.Number != 0 && nb.Number < nb.MinimumValue)
                            nb.Number = -nb.Number;
                        break;

                    case NumberBoxRange.OnlyNegative:

                        if(nb.MaximumValue > 0)
                            nb.MaximumValue = 0;

                        if (nb.Number != 0 && nb.Number > nb.MaximumValue)
                            nb.Number = -nb.Number;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the current number to <see cref="MaximumValue"/> if the current number is bigger than the newly set maximum value
        /// </summary>
        /// <param name="d">the number box which had its <see cref="MaximumValue"/> changed</param>
        /// <param name="e">the event arguments</param>
        private static void MaxValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                if (string.IsNullOrEmpty(nb.Text))
                    return;

                if (nb.MaximumValue < nb.MinimumValue)
                    throw new Exception($"{nameof(MaximumValue)} cannot be lesser than {nameof(MinimumValue)}");

                if (nb.MaximumValue < double.Parse(nb.Text))
                    nb.Text = nb.MaximumValue.ToString();
            }
        }

        /// <summary>
        /// Sets the current number to <see cref="MinimumValue"/> if the current number is bigger than the newly set maximum value
        /// </summary>
        /// <param name="d">the <see cref="NumberBox"/> which had its <see cref="MinimumValue"/> changed</param>
        /// <param name="e">the event arguments</param>
        private static void MinValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                if (string.IsNullOrEmpty(nb.Text))
                    return;

                if (nb.MaximumValue < nb.MinimumValue)
                    throw new Exception($"{nameof(MinimumValue)} cannot be more than {nameof(MaximumValue)}");

                //Set the value to minimum value if the current value is lower than the minimum value
                if (nb.MinimumValue > double.Parse(nb.Text))
                    nb.Text = nb.MinimumValue.ToString();
            }
        }

        private static void AllowDecimalChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                if (string.IsNullOrEmpty(nb.Text))
                    return;

                //remove the decimal and convert to int if allow decimal is changed to false
                int number = int.Parse(nb.Text);

                if (nb.AllowDecimal is false)
                    nb.Text = number.ToString();
            }
        }

        #endregion Private methods

        #region Regex

        [GeneratedRegex("^[-]?[0-9]*[.]?[0-9]*$")]
        private static partial Regex PositiveNegatieDecimalNumber();

        [GeneratedRegex("^[-]?[0-9]*$")]
        private static partial Regex PositiveNegativeInteger();

        [GeneratedRegex("^[0-9]*[.]?[0-9]*$")]
        private static partial Regex PositiveDecimalNumber();

        [GeneratedRegex("^[0-9]*$")]
        private static partial Regex PositiveInteger();

        [GeneratedRegex("^[-][0-9]*[.]?[0-9]*$")]
        private static partial Regex NegativeDecimalNumber();

        [GeneratedRegex("^[-][0-9]*$")]
        private static partial Regex NegativeInteger();

        #endregion Regex
    }
}