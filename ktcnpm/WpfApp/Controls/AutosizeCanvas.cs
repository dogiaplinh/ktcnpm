using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Controls
{
    public class AutosizeCanvas : Canvas
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size size;
            size = base.MeasureOverride(constraint);
            if (InternalChildren.Count == 0)
                return size;
            double width = InternalChildren
                .OfType<FrameworkElement>()
                .Max(i => i.ActualWidth + (double)i.GetValue(LeftProperty));

            double height = InternalChildren
                .OfType<FrameworkElement>()
                .Max(i => i.ActualHeight + (double)i.GetValue(TopProperty));
            if (double.IsNaN(width) || double.IsNaN(height))
                return size;
            return new Size(width, height);
        }
    }
}
