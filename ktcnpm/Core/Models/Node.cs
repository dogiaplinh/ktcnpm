using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Node : BindableBase
    {
        private static int s_counter = 0;
        private bool error;
        private NodeType type;

        public void CheckNode()
        {
            double probability = 0;
            foreach (var item in Paths)
            {
                item.Target.CheckNode();
                probability += item.Probability;
            }
            if (type == NodeType.Normal && probability < 1.0)
                Error = true;
        }

        public Node() : this(null)
        {
        }

        public Node(Node parent)
        {
            Id = s_counter++;
            Paths = new List<PathItem>();
            Parent = parent;
        }

        public bool Error
        {
            get { return error; }
            set { SetProperty(ref error, value, nameof(Error)); }
        }

        public int Id { get; set; }

        public Node Parent { get; }

        public PathItem ParentPath { get; set; }

        public List<PathItem> Paths { get; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value, nameof(Type)); }
        }
    }
}