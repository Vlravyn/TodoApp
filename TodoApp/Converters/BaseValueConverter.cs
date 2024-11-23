using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TodoApp.Converters
{
    /// <summary>
    /// A base value converter that every converter can inherit from
    /// </summary>
    /// <typeparam name="T">the type of converter that is inheriting from this class</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        private T converter;

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider) => converter ??= new T();
    }
}