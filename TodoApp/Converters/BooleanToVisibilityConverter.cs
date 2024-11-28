using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.Converters
{
    /// <summary>
    /// Converts a <see cref="bool"/> to <see cref="Visibility"/>.
    /// Pass <see langword="true"/> as parameter to use inverted version.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value is bool b)
            {
                //use inverted version if true is passed as parameter
                if(parameter is bool isInverted && isInverted)
                    return b ? Visibility.Collapsed : Visibility.Visible;
                else
                    return b ? Visibility.Visible : Visibility.Collapsed;
            }

            throw new ArgumentException($"unknown type passed in to {nameof(BooleanToVisibilityConverter)}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
