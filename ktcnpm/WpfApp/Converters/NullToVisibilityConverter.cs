﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
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
