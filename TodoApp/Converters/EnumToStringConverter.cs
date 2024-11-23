using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Data;
using TodoApp.Core;

namespace TodoApp.Converters
{
    /// <summary>
    /// Converts Enum to string and vice versa.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringConverter : BaseValueConverter<EnumToStringConverter>
    {
        /// <summary>
        /// Converts an enum to string
        /// </summary>
        /// <param name="value">the enum value to convert</param>
        /// <param name="parameter">the enum value to convert. Can send value from both parameters, but this one takes the precedence</param>
        /// <returns>a string</returns>
        /// <exception cref="ArgumentException">thrown when <paramref name="value"/> is an unknown type</exception>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum enumValue = (Enum)parameter ?? (Enum)value;

            if (enumValue is null)
                throw new ArgumentException("unknown type given as value to EnumToStringConverter");

            return enumValue.ToEnumStringValue();
        }

        /// <summary>
        /// Converts a string to enum value
        /// </summary>
        /// <param name="value">the string value to convert</param>
        /// <param name="targetType">the target enum type</param>
        /// <returns>the enum value</returns>
        /// <exception cref="ArgumentException">thrown when <paramref name="targetType"/> is not an enum</exception>
        /// <exception cref="InvalidOperationException">thrown when parsing to enum fails</exception>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!targetType.IsEnum)
                throw new ArgumentException("TargetType must be an enum");

            var stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
                throw new ArgumentException("cannot convert null or empty string to enum");

            var enumvalues = Enum.GetValues(targetType).Cast<Enum>().ToList();

            if (enumvalues == null || enumvalues.Count == 0)
                throw new ArgumentException("Failed to get enum values or enum contains no values");

            foreach (var enumValue in enumvalues)
            {
                var attribute = enumValue.GetAttribute<EnumMemberAttribute>();

                if (attribute is not null && attribute.Value == stringValue)
                    return Enum.Parse(targetType, stringValue);
            }

            throw new Exception("unknown error occured while converting string to enum");
        }
    }
}