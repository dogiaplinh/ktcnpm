using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Node : BindableBase
    {
        private static int s_counter = 0;
        private NodeType type;

        public Node() : this(null)
        {
        }

        public Node(Node parent)
        {
            Id = s_counter++;
            Children = new List<Node>();
            Parent = parent;
        }

        public List<Node> Children { get; private set; }

        public int Id { get; set; }

        public Node Parent { get; private set; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }
    }
}