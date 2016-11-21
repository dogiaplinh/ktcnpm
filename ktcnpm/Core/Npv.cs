using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Core
{
    public static class Npv
    {
        private static double rate = 0.1;
        /// <summary>
        /// Tính NPV dựa theo chi phí, doanh thu từng năm
        /// </summary>
        /// <param name="costs">Chi phí từng năm</param>
        /// <param name="revenues">Doanh thu từng năm</param>
        /// <param name="rate">Lãi suất ngân hàng cố định. Đơn vị %</param>
        /// <returns></returns>
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

        /// <summary>
        /// Tính NPV dựa theo chi phí, doanh thu từng năm
        /// </summary>
        /// <param name="costs">Chi phí từng năm</param>
        /// <param name="revenues">Doanh thu từng năm</param>
        /// <param name="rate">Lãi suất ngân hàng từng năm. Đơn vị %</param>
        public static double CalculateNpv(double[] costs, double[] revenues, double[] rates)
        {
            double cost = 0;
            if (rates.Length + 1 < costs.Length || rates.Length + 1 < revenues.Length)
                throw new ArgumentException();
            for (int i = costs.Length - 1; i >= 0; i--)
            {
                if (i < costs.Length - 1)
                    cost /= (1 + rates[i] / 100);
                cost -= costs[i];
            }
            double revenue = 0;
            for (int i = revenues.Length - 1; i >= 0; i--)
            {
                if (i < costs.Length - 1)
                    revenue /= (1 + rates[i] / 100);
                revenue += revenues[i];
            }
            return cost + revenue;
        }

        /// <summary>
        /// Liệt kê tất cả các hướng quyết định
        /// </summary>
        /// <param name="root">Nút gốc</param>
        /// <returns>Danh sách path id tại các điểm rẽ</returns>
        /// <remarks>
        /// Hàm này chưa chạy đúng, cần phải sửa
        /// </remarks>
        public static List<RouteItem> ListAllPaths(Node root)
        {
            Dictionary<string, double> npv = new Dictionary<string, double>();
            List<string> paths = root.getDecisionPath();
            List<RouteItem> routes = new List<RouteItem>();
            foreach (string item in paths)
            {
                var route = new RouteItem(item);
                int temp = 0;
                string str = null;
                RouteHelper.ReadPathNames(root, route.Paths.Reverse<int>().ToList(), ref temp, ref str);
                route.Description = str;
                routes.Add(route);
            }
            return routes;
        }

        public static double CalculateNpv(Node node, RouteItem route)
        {
            double npv = 0;
            List<int> paths = route.Paths;
            List<PathItem> ListPath = new List<PathItem>();
            foreach (PathItem p in node.Paths)
            {
                if (paths.Contains(p.Id))
                {
                    p.Probability = 1;
                    p.AvgProb = 1;
                    ListPath.Add(p);
                }
            }
            bool isFinish = false;
            while (!isFinish)
            {
                isFinish = true;
                List<PathItem> tmp = new List<PathItem>(ListPath);
                foreach (PathItem pItem in tmp)
                {
                    if (pItem.Check == true) continue;
                    if (pItem.Target.Type == NodeType.Normal)
                    {
                        foreach (PathItem p in pItem.Target.Paths)
                        {
                            if (p.Type == NodeType.End) continue;
                            p.Depth = pItem.Depth + 1;
                            p.AvgProb = p.Probability == 0 ? 1 : p.Probability;
                            p.AvgProb *= pItem.AvgProb;
                            p.Type = NodeType.Normal;
                            ListPath.Add(p);
                            isFinish = false;
                        }
                        pItem.Check = true;
                    }
                    else if (pItem.Target.Type == NodeType.Decision)
                    {
                        foreach (PathItem p in pItem.Target.Paths)
                        {
                            if (paths.Contains(p.Id))
                            {
                                p.Depth = pItem.Depth;
                                p.AvgProb = p.Probability == 0 ? 1 : p.Probability;
                                p.AvgProb *= pItem.AvgProb;
                                p.Type = NodeType.Decision;
                                ListPath.Add(p);
                                isFinish = false;
                            }
                        }
                        pItem.Check = true;
                    }
                }
            }
            foreach (PathItem p in ListPath)
            {
                p.Check = false;
                double prob = p.Type == NodeType.Decision ? 1 : p.AvgProb;
                npv += p.Cost * prob / Math.Pow(rate + 1, p.Depth);
            }
            return npv;
        }
    }
}