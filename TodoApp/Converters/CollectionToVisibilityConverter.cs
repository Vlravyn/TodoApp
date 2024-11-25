using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.Converters
{
    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if the <see cref="ICollection"/> is not null or empty.
    /// Takes an optional parameter, which, if <see langword="true"/> will return <see cref="Visibility.Visible"/> only when collection is null or empty
    /// </summary>
    [ValueConversion(typeof(ICollection), typeof(Visibility))]
    public class CollectionToVisibilityConverter : BaseValueConverter<CollectionToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isInverted = (bool?)parameter;
            var converted = (ICollection)value;

            if (converted is null)
            {
                return isInverted switch
                {
                    true => Visibility.Visible,
                    _ => Visibility.Collapsed,
                };
            }
            else
            {
                return isInverted switch
                {
                    true => converted.Count == 0 ? Visibility.Visible : Visibility.Collapsed,
                    _ => converted.Count != 0 ? Visibility.Visible : Visibility.Collapsed,
                };
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}