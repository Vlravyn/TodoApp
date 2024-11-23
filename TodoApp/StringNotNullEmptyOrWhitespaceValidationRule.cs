using System.Globalization;
using System.Windows.Controls;

namespace TodoApp
{
    public class StringNotNullEmptyOrWhitespaceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string str)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                    return new ValidationResult(false, "title is null or empty or whitespace");

                return new ValidationResult(true, "title is valid");
            }

            throw new Exception("validation can only be done on strings");
        }
    }
}