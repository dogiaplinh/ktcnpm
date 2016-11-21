using Core.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
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
        private int max = 0;

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
            Node node = new Node
            {
                Type = type
            };
            NodeControl nodeControl = new NodeControl
            {
                DataContext = node
            };
            nodeControl.AddNewNode += NodeControl_AddNewNode;
            node.CanvasLeft = Canvas.GetLeft(parent) + (node.Type == parentNode.Type ? 400 : 200);
            node.CanvasTop = Canvas.GetTop(parent);

            if ((node.CanvasLeft + 150) / 400 > max)
            {
                max++;
                DrawYearLine(max);
            }

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
            nodeControl.DeleteNode += (s, e) =>
            {
                parentNode.Paths.Remove(path);
                canvas.Children.Remove(nodeControl);
                canvas.Children.Remove(pathControl);
            };

            canvas.Children.Add(nodeControl);
            canvas.Children.Add(pathControl);
            CreateDragDrop(nodeControl);
        }

        private void DrawYearLine(int year)
        {
            YearControl yearControl = new YearControl();
            yearControl.DataContext = new YearItem() { Year = year };
            Canvas.SetLeft(yearControl, 400 * year - 150);
            Binding binding = new Binding("ActualHeight");
            binding.Source = canvas;
            yearControl.SetBinding(HeightProperty, binding);
            canvas.Children.Add(yearControl);
        }

        private NodeControl CreateRootNode()
        {
            NodeControl nodeControl = new NodeControl();
            Binding binding = new Binding("Root");
            binding.Source = DataContext;
            nodeControl.SetBinding(DataContextProperty, binding);
            nodeControl.AddNewNode += NodeControl_AddNewNode;
            viewModel.Root.CanvasLeft = 50;
            viewModel.Root.CanvasTop = canvas.ActualHeight / 2 - 30;
            canvas.Children.Add(nodeControl);
            CreateDragDrop(nodeControl);
            return nodeControl;
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
                //Canvas.SetLeft(control, left);
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

        private void LoadTree(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            var nodeControl = CreateRootNode();
            viewModel.LoadTree();
            foreach (var path in viewModel.Root.Paths)
            {
                path.Source = viewModel.Root;
                CreateNodeRecursive(nodeControl, path);
            }
        }

        private void CreateNodeRecursive(NodeControl parent, PathItem path)
        {
            Node parentNode = parent.DataContext as Node;
            path.Target.ParentPath = path;
            NodeControl nodeControl = new NodeControl
            {
                DataContext = path.Target
            };
            if (parentNode.ParentPath != null)
                parentNode.CanvasLeft = parentNode.ParentPath.Source.CanvasLeft + (parentNode.ParentPath.Source.Type == parentNode.Type ? 400 : 200);
            else parentNode.CanvasLeft = 50;

            if ((parentNode.CanvasLeft + 150) / 400 > max)
            {
                max++;
                DrawYearLine(max);
            }
            nodeControl.AddNewNode += NodeControl_AddNewNode;
            canvas.Children.Add(nodeControl);
            foreach (var p in path.Target.Paths)
            {
                p.Source = parentNode;
                CreateNodeRecursive(nodeControl, p);
            }
            

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
            nodeControl.DeleteNode += (s, e) =>
            {
                parentNode.Paths.Remove(path);
                canvas.Children.Remove(nodeControl);
                canvas.Children.Remove(pathControl);
            };

            canvas.Children.Add(pathControl);
            CreateDragDrop(nodeControl);
        }
    }
}