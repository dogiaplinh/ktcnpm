using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class RouteItem
    {
        public List<int> Paths { get; private set; }

        public string Description { get; set; }

        public RouteItem(List<int> paths)
        {
            Paths = new List<int>(paths);
        }
    }
}