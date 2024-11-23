using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TodoApp.Core.DataModels;

namespace TodoApp.Converters
{
    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if the <see cref="UserTask.Steps"/> is not null or empty.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<Step>), typeof(Visibility))]
    public class StepsToVisibilityConverter : BaseValueConverter<StepsToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converted = (IEnumerable<Step>)value;
            if (converted is null)
                return Visibility.Collapsed;
            else if (converted is IEnumerable<Step> steps)
                return steps.Any() ? Visibility.Visible : Visibility.Collapsed;

            throw new Exception("unknown error occured while converting IEnumerable<Step> to visibility");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}