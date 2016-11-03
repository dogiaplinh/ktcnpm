using Core.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp.Converters
{
    public class NodeTypeToBrushConverter : IValueConverter
    {
        private static Brush normalBrush = new SolidColorBrush(Parameters.NormalColor);
        private static Brush endBrush = new SolidColorBrush(Parameters.EndColor);
        private static Brush decisionBrush = new SolidColorBrush(Parameters.DecisionColor);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (NodeType)value;
            switch (type)
            {
                case NodeType.Normal:
                    return normalBrush;
                case NodeType.Decision:
                    return decisionBrush;
                case NodeType.End:
                    return endBrush;
                default:
                    return endBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}