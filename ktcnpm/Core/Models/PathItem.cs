using System;
using System.Linq;

namespace Core.Models
{
    public class PathItem : BindableBase
    {
        private static int s_counter = 0;
        private double cost;
        private string name;
        private double probability;
        private NodeType type;

        public PathItem()
        {
            Id = s_counter++;
        }

        public PathItem(Node source, Node target) : this()
        {
            Source = source;
            Target = target;
            Source.Paths.Add(this);
            Target.ParentPath = this;
            if (target.Type == NodeType.End)
                Probability = 1;
        }

        public double Cost
        {
            get { return cost; }
            set { SetProperty(ref cost, value, nameof(Cost)); }
        }

        public int Id { get; }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value, nameof(Name)); }
        }

        public double Probability
        {
            get { return probability; }
            set { SetProperty(ref probability, value, nameof(Probability)); }
        }

        public Node Source { get; }

        public Node Target { get; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value, nameof(Type)); }
        }
    }
}