using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private int depth = 0;
        private bool select = false;
        public bool Select
        {
            get { return select; }
            set { SetProperty(ref select, value); }
        }
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        private bool check = false;

        public bool Check
        {
            get { return check; }
            set { check = value; }
        }
        private double avgprob = 1;

        public double AvgProb
        {
            get { return avgprob; }
            set { avgprob = value; }
        }

        public PathItem()
        {
            Id = s_counter++;
        }

        public PathItem(Node source, Node target)
            : this()
        {
            Source = source;
            Target = target;
            Source.Paths.Add(this);
            Target.ParentPath = this;
            if (target.Type == NodeType.End || source.Type == NodeType.Decision)
                this.probability = 1;
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
        public Node Source { get; set; }

        public Node Target { get; set; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }
    }
}