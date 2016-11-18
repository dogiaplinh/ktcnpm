using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

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

        // Get decision path
        public List<string> getDecisionPath()
        {   
            // Node ket thuc 
            if (this.Type == NodeType.End) return null;

            // Danh sach quyet dinh cua cac nut con
            Dictionary<PathItem, List<string>> child = new Dictionary<PathItem, List<string>>();
            foreach(PathItem item in this.Paths){
                List<string> d = item.Target.getDecisionPath();
                if (d != null) child[item] = d;
            }

            // Ket hop danh sach quyet dinh cua cac nut con
            List<string> decisionPath = new List<string>();
            //Neu nut la nut quyet dinh, them path quyet dinh vao day
            if (this.Type == NodeType.Decision)
            {   
                // Node quyet dinh dau tien (ko co node quyet dinh con)
                if (child.Count == 0)
                {
                    foreach (PathItem item in this.Paths)
                    {
                        decisionPath.Add(item.Id.ToString());
                    }
                }
                else        // Node quyet dinh co node quyet dinh con
                {
                    foreach (PathItem item in this.Paths)
                    {
                        if (!child.ContainsKey(item))
                        {
                            decisionPath.Add(item.Id.ToString());
                        }
                        else
                        {

                            foreach (string path in child[item])
                            {
                                decisionPath.Add(path + "_" + item.Id);
                            }
                        }
                    }
                }
            }
            else if (this.Type == NodeType.Normal)      // Neu node la node thuong, ket hop cac quyet dinh cua cac node con
            {
                if (child.Count == 0) return null;
                else if (child.Count == 1)
                {
                    foreach (PathItem item in child.Keys)
                    {
                        return child[item];
                    }
                }
                else
                {
                    foreach (List<string> item in child.Values)
                    {
                        decisionPath = multiplyList(decisionPath, item);
                    }
                }

            }
            return decisionPath;
        }

        private List<string> multiplyList(List<string> l1, List<string> l2)
        {
            List<string> mul = new List<string>();
            if (l1.Count == 0) return l2;
            if (l2.Count == 0) return l1;
            foreach (string s1 in l1)
            {
                foreach (string s2 in l2)
                    mul.Add(s1 + "_" + s2);
            }
            return mul;
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