﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value is bool b)
                return b ? Visibility.Visible : Visibility.Collapsed;

            throw new ArgumentException($"unknown type passed in to {nameof(InvertedBooleanToVisibilityConverter)}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
