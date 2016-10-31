using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Controls
{
    public partial class PathControl : UserControl
    {
        public static readonly DependencyProperty PositionsProperty =
            DependencyProperty.Register("Positions", typeof(double[]), typeof(PathControl), new PropertyMetadata(null, OnPositionsChanged));

        public PathControl()
        {
            InitializeComponent();
        }

        public double[] Positions
        {
            get { return (double[])GetValue(PositionsProperty); }
            set { SetValue(PositionsProperty, value); }
        }

        private static void OnPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pathControl = d as PathControl;
            pathControl.RefreshPosition();
        }

        private void RefreshPosition()
        {
            double x1 = Positions[0];
            double y1 = Positions[1];
            double x2 = Positions[2];
            double y2 = Positions[3];
            double dx = x2 - x1;
            double dy = y2 - y1;
            double angle = 0;
            if (dx == 0)
                angle = dy > 0 ? 90 : 270;
            else angle = Math.Atan(dy / dx) * 180 / Math.PI;
            double length = Math.Sqrt(dx * dx + dy * dy);
            Width = Math.Max(length - 90, 0);
            rotateTransform.Angle = angle;
            Canvas.SetLeft(this, (x1 + x2 - Width) / 2 + 30);
            Canvas.SetTop(this, (y1 + y2 - 50) / 2 + 30);
        }
    }
}