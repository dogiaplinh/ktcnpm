using Core.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp.Controls;
using WpfApp.Converters;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private object movingObject;
        private double firstLeft;
        private double firstTop;
        private NodePositionsConverter converter = new NodePositionsConverter();
        private MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateRootNode();
        }

        private void CreateChildNode(NodeControl parent, NodeType type)
        {
            Node parentNode = parent.DataContext as Node;
            Node node = new Node(parentNode)
            {
                Type = type
            };
            NodeControl nodeControl = new NodeControl
            {
                DataContext = node
            };
            nodeControl.AddNewNode += NodeControl_AddNewNode;
            Canvas.SetLeft(nodeControl, Canvas.GetLeft(parent) + 200);
            Canvas.SetTop(nodeControl, Canvas.GetTop(parent));

            PathItem path = new PathItem(parentNode, node)
            {
                Type = parentNode.Type
            };
            PathControl pathControl = new PathControl
            {
                DataContext = path
            };
            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Converter = converter;
            multiBinding.Bindings.Add(new Binding("(Canvas.Left)") { Source = parent });
            multiBinding.Bindings.Add(new Binding("(Canvas.Top)") { Source = parent });
            multiBinding.Bindings.Add(new Binding("(Canvas.Left)") { Source = nodeControl });
            multiBinding.Bindings.Add(new Binding("(Canvas.Top)") { Source = nodeControl });
            multiBinding.NotifyOnSourceUpdated = true;
            pathControl.SetBinding(PathControl.PositionsProperty, multiBinding);
            pathControl.MouseDown += (s, e) =>
            {
                viewModel.Selected = ((FrameworkElement)s).DataContext;
            };

            canvas.Children.Add(nodeControl);
            canvas.Children.Add(pathControl);
            CreateDragDrop(nodeControl);
        }

        private void CreateRootNode()
        {
            NodeControl nodeControl = new NodeControl
            {
                DataContext = viewModel.Root
            };
            nodeControl.AddNewNode += NodeControl_AddNewNode;
            Canvas.SetLeft(nodeControl, 10);
            Canvas.SetTop(nodeControl, canvas.ActualHeight / 2 - 30);
            canvas.Children.Add(nodeControl);
            CreateDragDrop(nodeControl);
        }

        private void NodeControl_AddNewNode(object sender, NodeType e)
        {
            CreateChildNode(sender as NodeControl, e);
        }

        private void CreateDragDrop(UIElement control)
        {
            control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
            control.MouseMove += Control_MouseMove;
            control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            movingObject = null;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                Point p = e.GetPosition(null);
                var control = sender as UIElement;
                double left = Canvas.GetLeft(control);
                double top = Canvas.GetTop(control);
                top = firstTop + p.Y - startPoint.Y;
                left = firstLeft + p.X - startPoint.X;
                Canvas.SetLeft(control, left);
                Canvas.SetTop(control, top);
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            movingObject = sender;

            var control = sender as UIElement;
            firstLeft = Canvas.GetLeft(control);
            firstTop = Canvas.GetTop(control);

            int top = 0;
            foreach (UIElement child in canvas.Children)
                if (top < Panel.GetZIndex(child))
                    top = Panel.GetZIndex(child);
            Panel.SetZIndex(sender as UIElement, top + 1);
        }

        private void DebugMethod([CallerMemberName] string name = null)
        {
            Debug.WriteLine(name);
        }
    }
}