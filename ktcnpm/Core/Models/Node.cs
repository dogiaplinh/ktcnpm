using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Models
{
    public class Node : BindableBase
    {
        private static int s_counter = 0;
        private NodeType type;

        public Node()
        {
            Id = s_counter++;
            Paths = new ObservableCollection<PathItem>();
        }

        public Node(int id)
        {
            Id = id;
            s_counter = id + 1;
            Paths = new ObservableCollection<PathItem>();
        }

        public int Id { get; set; }

        [JsonIgnore]
        public PathItem ParentPath { get; set; }

        public ObservableCollection<PathItem> Paths { get; private set; }

        public NodeType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

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

        #region For UI

        private bool error;

        public double CanvasLeft { get; set; }

        public double CanvasTop { get; set; }

        [JsonIgnore]
        public bool Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        #endregion For UI
    }
}