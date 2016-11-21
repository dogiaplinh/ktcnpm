using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class RouteItem : BindableBase
    {
        private double npv;

        public RouteItem(List<int> paths)
        {
            Paths = new List<int>(paths);
        }

        public RouteItem(string paths)
        {
            Paths = new List<int>();
            foreach (string item in paths.Split('_'))
            {
                Paths.Add(int.Parse(item));
            }
        }

        public string Description { get; set; }

        public double Npv
        {
            get { return npv; }
            set { SetProperty(ref npv, value); }
        }

        public List<int> Paths { get; private set; }

        public override string ToString()
        {
            string str = "";
            foreach (int item in Paths)
            {
                str += item.ToString() + " ";
            }
            return str;
        }
    }
}