using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class IntegerToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
            {
                if (parameter == null || parameter.ToString() == "Collapsed")
                    return Visibility.Collapsed;
                else if (parameter.ToString() == "Visible")
                    return Visibility.Visible;
                return DependencyProperty.UnsetValue;
            }
            else
            {
                if (parameter == null || parameter.ToString() == "Collapsed")
                    return Visibility.Visible;
                else if (parameter.ToString() == "Visible")
                    return Visibility.Collapsed;
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}