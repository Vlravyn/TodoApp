using System.Runtime.Serialization;

namespace TodoApp.Core
{
    public static class HelperMethods
    {
        /// <summary>
        /// Get the first attribute of specific type
        /// </summary>
        /// <typeparam name="T">the type of attribute to get</typeparam>
        /// <param name="value">the value whose attribute we need to get</param>
        /// <returns>the attribute. Returns null if attribute of given type is not assigned to the value</returns>
        public static T? GetAttribute<T>(this object value)
            where T : Attribute, new()
        {
            if (value == null)
                return null;

            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attribute = memInfo[0].GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attribute == null)
                return null;

            return attribute as T;
        }

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

            return attributes[0] as T;
        }

        /// <summary>
        /// Gets teh <see cref="EnumMemberAttribute"/> value for this enum
        /// </summary>
        /// <param name="value">the value to get the enum member of</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">thrown when the value of this enum member is null or empty</exception>
        /// <exception cref="ArgumentNullException"></exception>
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