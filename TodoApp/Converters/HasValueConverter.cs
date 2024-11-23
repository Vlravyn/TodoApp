using System.Globalization;
using System.Windows.Data;

namespace TodoApp.Converters
{
    /// <summary>
    /// Checks if the value actually has a value and is not null.
    /// Returns <see langword="true"/> if the object has value.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class HasValueConverter : BaseValueConverter<HasValueConverter>
    {
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        /// <summary>
        /// NOT IMPLEMENTED.
        /// </summary>
        /// <exception cref="NotImplementedException">thrown because the method is not implemented.</exception>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}