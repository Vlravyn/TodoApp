using System.Globalization;
using System.Windows.Data;

namespace TodoApp.Converters
{
    [ValueConversion(typeof(bool), typeof(int))]
    public class MinHourValueConverter : BaseValueConverter<MinHourValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            if(value is bool b)
            {
                if(b)
                    return 0;
                else
                    return 1;
            }

            throw new Exception($"Unknown type passed in to {nameof(MinHourValueConverter)}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
