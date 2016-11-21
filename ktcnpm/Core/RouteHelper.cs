using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class RouteHelper
    {
        private Node root;
        private List<RouteItem> routes = new List<RouteItem>();

        public RouteHelper(Node root)
        {
            this.root = root;
        }

        public List<RouteItem> ListAllRoutes()
        {
            int index = 0;
            bool error = false;
            var list = new List<int>();
            int current = 0;
            do
            {
                error = false;
                current = 0;
                ListAllRoutesRecursive(root, list, ref current, ref index, ref error);
                if (list.Count == 0)
                    break;
            }
            while (index >= 0);
            Debug.WriteLine(JsonConvert.SerializeObject(routes));
            return routes;
        }

        public static void ReadPathNames(Node node, List<int> list, ref int current, ref string text)
        {
            switch (node.Type)
            {
                case NodeType.Normal:
                    foreach (var p in node.Paths)
                    {
                        ReadPathNames(p.Target, list, ref current, ref text);
                    }
                    break;

                case NodeType.Decision:
                    if (node.Paths.Count == 0)
                        break;
                    int currentBranch = current < list.Count ? list[current] : -1;
                    int i;
                    for (i = 0; i < node.Paths.Count; i++)
                    {
                        if (node.Paths[i].Id == currentBranch)
                            break;
                    }
                    if (text == null)
                        text = node.Paths[i].Name;
                    else text += ", " + node.Paths[i].Name;
                    current++;
                    ReadPathNames(node.Paths[i].Target, list, ref current, ref text);
                    break;

                case NodeType.End:
                    break;
            }
        }

        private void ListAllRoutesRecursive(Node node, List<int> list, ref int current, ref int index, ref bool error)
        {
            if (error) return;
            switch (node.Type)
            {
                case NodeType.Normal:
                    foreach (var p in node.Paths)
                    {
                        ListAllRoutesRecursive(p.Target, list, ref current, ref index, ref error);
                    }
                    break;

                case NodeType.Decision:
                    if (node.Paths.Count == 0)
                        break;
                    int currentBranch = current < list.Count ? list[current] : -1;
                    var decision = node.Paths.Select(x => x.Id).Where(x => list.Contains(x));
                    if (decision.Count() == 0)
                    {
                        list.Add(node.Paths[0].Id);
                        current++;
                        index++;
                        ListAllRoutesRecursive(node.Paths[0].Target, list, ref current, ref index, ref error);
                    }
                    else if (current < index - 1)
                    {
                        int i;
                        for (i = 0; i < node.Paths.Count; i++)
                        {
                            if (node.Paths[i].Id == currentBranch)
                                break;
                        }
                        current++;
                        ListAllRoutesRecursive(node.Paths[i].Target, list, ref current, ref index, ref error);
                    }
                    else
                    {
                        int n = decision.First();
                        int i;
                        for (i = 0; i < node.Paths.Count; i++)
                        {
                            if (node.Paths[i].Id == n)
                                break;
                        }
                        if (i == node.Paths.Count - 1)
                        {
                            error = true;
                            if (list.Count > 0)
                                list.RemoveAt(list.Count - 1);
                            index--;
                        }
                        else
                        {
                            list[list.Count - 1] = node.Paths[i + 1].Id;
                            ListAllRoutesRecursive(node.Paths[i + 1].Target, list, ref current, ref index, ref error);
                        }
                    }
                    break;

                case NodeType.End:
                    break;
            }
            if (node.Id == root.Id && !error && list.Count > 0)
            {
                var routeItem = new RouteItem(list);
                routes.Add(routeItem);
                int temp = 0;
                string str = null;
                ReadPathNames(node, list, ref temp, ref str);
                routeItem.Description = str;
            }
        }
    }
}