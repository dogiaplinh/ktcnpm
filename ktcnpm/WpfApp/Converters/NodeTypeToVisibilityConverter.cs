using Core.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class NodeTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (NodeType)value;
            if (type.ToString() == parameter.ToString())
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
