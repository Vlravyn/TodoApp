using System.Globalization;
using System.Windows.Data;
using TodoApp.Core.DataModels;

namespace TodoApp.Converters
{
    [ValueConversion(typeof(IEnumerable<Step>), typeof(uint))]
    public class StepsToCompletedCountConverter : BaseValueConverter<StepsToCompletedCountConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converted = (IEnumerable<Step>)value;

            if (converted is null)
                throw new ArgumentException($"unknown type sent to {nameof(StepsToCompletedCountConverter)}");

            return converted.TakeWhile(t => t.IsCompleted).Count();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}