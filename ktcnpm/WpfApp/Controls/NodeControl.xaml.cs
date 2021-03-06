﻿using Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Controls
{
    public partial class NodeControl : UserControl
    {
        public NodeControl()
        {
            InitializeComponent();
        }

        public event EventHandler<NodeType> AddNewNode;

        public event EventHandler DeleteNode;

        private void AddDecisionNode(object sender, RoutedEventArgs e)
        {
            if (AddNewNode != null)
                AddNewNode(this, NodeType.Decision);
        }

        private void AddEndNode(object sender, RoutedEventArgs e)
        {
            if (AddNewNode != null)
                AddNewNode(this, NodeType.End);
        }

        private void AddNormalNode(object sender, RoutedEventArgs e)
        {
            if (AddNewNode != null)
                AddNewNode(this, NodeType.Normal);
        }

        private void ChangeToDecision(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            if (node != null)
            {
                node.Type = NodeType.Decision;
                foreach (var item in node.Paths)
                {
                    item.Type = NodeType.Decision;
                }
            }
        }

        private void ChangeToEnd(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            if (node != null)
            {
                node.Type = NodeType.End;
                foreach (var item in node.Paths)
                {
                    item.Type = NodeType.End;
                }
            }
        }

        private void ChangeToNormal(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            if (node != null)
            {
                node.Type = NodeType.Normal;
                foreach (var item in node.Paths)
                {
                    item.Type = NodeType.Normal;
                }
            }
        }
        private void DeleteThisNode(object sender, RoutedEventArgs e)
        {
            if (DeleteNode != null)
                DeleteNode(this, null);
        }
    }
}