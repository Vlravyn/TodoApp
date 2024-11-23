using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.Converters
{
    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if the text of the textbox is <see langword="null"/> or empty
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class PlaceholderTextVisibilityConverter : BaseValueConverter<PlaceholderTextVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string str)
            {
                if(string.IsNullOrEmpty(str))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
