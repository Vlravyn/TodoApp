using System.Runtime.Serialization;

namespace TodoApp.Core
{
    public static class HelperMethods
    {
        /// <summary>
        /// Attempts to get the attribute T for the enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum value)
            where T : Attribute, new()
        {
            if (value == null)
                return null;

            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            if (attributes == null)
                return null;

            return (T)attributes[0];
        }

        /// <summary>
        /// Gets teh <see cref="EnumMemberAttribute"/> value for this enum
        /// </summary>
        /// <param name="value">the value to get the enum member of</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">thrown when the value of this enum member is null or empty</exception>
        /// <exception cref="ArgumentNullException">thrown when null was passed as parameter</exception>
        public static string ToEnumStringValue(this Enum value)
        {
            ArgumentNullException.ThrowIfNull(value);

            var attribute = value.GetAttribute<EnumMemberAttribute>();

            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
                throw new ArgumentException("Enum string value not set");

            return attribute.Value;
        }
    }
}