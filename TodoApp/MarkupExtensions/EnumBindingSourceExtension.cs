using System.Windows.Markup;

namespace TodoApp
{
    /// <summary>
    /// Allows to use Enum values as ItemsSource.
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; private set; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
                throw new ArgumentException("Type must be a type of enum");

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(EnumType);
    }
}
