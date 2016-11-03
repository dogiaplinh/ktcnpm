using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Npv
    {
        public static double CalculateNpv(double[] costs, double[] revenues, double rate)
        {
            double cost = 0;
            var scale = 1 + rate / 100;
            for (int i = costs.Length - 1; i >= 0; i--)
            {
                cost /= scale;
                cost -= costs[i];
            }
            double revenue = 0;
            for (int i = revenues.Length - 1; i >= 0; i--)
            {
                revenue /= scale;
                revenue += revenues[i];
            }
            return cost + revenue;
        }

        public static double CalculateNpv(double[] costs, double[] revenues, double[] rates)
        {
            double cost = 0;
            if (rates.Length < costs.Length || rates.Length < revenues.Length)
                throw new ArgumentException();
            for (int i = costs.Length - 1; i >= 0; i--)
            {
                cost /= (1 + rates[i] / 100);
                cost -= costs[i];
            }
            double revenue = 0;
            for (int i = revenues.Length - 1; i >= 0; i--)
            {
                revenue /= (1 + rates[i] / 100);
                revenue += revenues[i];
            }
            return cost + revenue;
        }

        public static List<List<int>> ListAllPaths(Node root, List<int> list = null)
        {
            List<List<int>> output = new List<List<int>>();
            if (list == null)
                list = new List<int>();
            switch (root.Type)
            {
                case NodeType.Normal:
                    foreach (var item in root.Paths)
                    {
                        output.AddRange(ListAllPaths(item.Target, list));
                    }
                    break;

                case NodeType.Decision:
                    foreach (var item in root.Paths)
                    {
                        var list2 = new List<int>(list);
                        output.AddRange(ListAllPaths(item.Target, list2));
                        output.Add(list2);
                    }
                    break;

                case NodeType.End:
                    return output;
            }
            return output;
        }
    }
}