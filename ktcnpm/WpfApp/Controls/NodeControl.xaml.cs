using Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Controls
{
    public partial class NodeControl : UserControl
    {
        public event EventHandler<NodeType> AddNewNode;

        public NodeControl()
        {
            InitializeComponent();
        }

        private void AddDecisionNode(object sender, RoutedEventArgs e)
        {
            AddNewNode?.Invoke(this, NodeType.Decision);
        }

        private void AddNormalNode(object sender, RoutedEventArgs e)
        {
            AddNewNode?.Invoke(this, NodeType.Normal);
        }

        private void ChangeToDecision(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            if (node != null)
            {
                node.Type = NodeType.Decision;
            }
        }

        private void ChangeToNormal(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            if (node != null)
            {
                node.Type = NodeType.Normal;
            }
        }
    }
}