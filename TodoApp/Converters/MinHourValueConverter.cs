using System.Globalization;
using System.Windows.Data;

namespace TodoApp.Converters
{
    /// <summary>
    /// Returns the minimum value that the <see cref="CustomControls.TimePicker.Hour"/> can have based on the <see cref="CustomControls.TimePicker.Is24HourFormat"/>
    /// is <see langword="true"/> or <see langword="false"/>
    /// </summary>
    [ValueConversion(typeof(bool), typeof(int))]
    public class MinHourValueConverter : BaseValueConverter<MinHourValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), $"The passed in value in {nameof(MinHourValueConverter)} cannot be null");

            if (value is bool b)
            {
                if(b)
                    return 0;
                else
                    return 1;
            }

            throw new InvalidOperationException($"Unknown type passed in to {nameof(MinHourValueConverter)}. The passed in value must be {nameof(Boolean)}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //This method is not used anywhere. I just created it because I felt like it.

            ArgumentNullException.ThrowIfNull($"The passed in value in {nameof(MinHourValueConverter)} cannot be null", nameof(value));

            if (value is int b)
            {
                if (b == 1)
                    return false;
                else
                    return true;
            }

            throw new InvalidOperationException($"Unknown type passed in to {nameof(MinHourValueConverter)}. The passed in value must be {nameof(Int32)}");
        }
    }
}
