using Newtonsoft.Json;
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

        /// <summary>
        /// Doanh thu/chi phí. Dấu dương tức là doanh thu, âm là chi phí
        /// </summary>
        public double Cost
        {
            get { return cost; }
            set { SetProperty(ref cost, value); }
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public double Probability
        {
            get { return probability; }
            set { SetProperty(ref probability, value); }
        }

        [JsonIgnore]
        public Node Source { get; private set; }

        public Node Target { get; set; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }
    }
}