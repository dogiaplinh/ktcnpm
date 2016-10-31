using System;
using System.Linq;

namespace Core.Models
{
    public class PathItem
    {
        public double Cost { get; set; }

        public string Name { get; set; }

        public NodeType Type { get; set; }

        public double Probability { get; set; }
    }
}