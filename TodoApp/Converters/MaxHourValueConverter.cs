using System.Globalization;
using System.Windows.Data;

namespace TodoApp.Converters
{
    [ValueConversion(typeof(bool), typeof(int))]
    public class MaxHourValueConverter : BaseValueConverter<MaxHourValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v)
            {
                if (v)
                    return 23;
                else
                    return 12;
            }

            throw new ArgumentException($"Invalid type passed in {nameof(MaxHourValueConverter)}. The value must a type of {nameof(Boolean)}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? v = (int?)value;
            if (v is not null)
            {
                if (v == 24)
                    return true;
                else
                    return false;
            }

            throw new ArgumentException($"invalid value paassed in {nameof(ConvertBack)} of {nameof(MaxHourValueConverter)}. Value must be numerical");
        }
    }
}