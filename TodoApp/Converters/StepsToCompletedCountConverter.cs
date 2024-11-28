using System.Globalization;
using System.Windows.Data;
using TodoApp.Core.DataModels;

namespace TodoApp.Converters
{
    /// <summary>
    /// Counts the number of <see cref="Step"/> completed in the <see cref="IEnumerable{T}"/> of <see cref="Step"/> and returns the number.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<Step>), typeof(uint))]
    public class StepsToCompletedCountConverter : BaseValueConverter<StepsToCompletedCountConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value), $"The passed in value in {nameof(StepsToCompletedCountConverter)} cannot be null.");

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