using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Npv
    {
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
        public static List<List<int>> ListAllPaths(Node root)
        {
            List<int> list = new List<int>();
            List<List<int>> output = ListAllPathsRecursive(root, list);
            for (int i = 0; i < output.Count; i++)
            {
                for (int j = output.Count - 1; j > i; j--)
                {
                    if (IsSubList(output[i], output[j]))
                    {
                        output.RemoveAt(j);
                    }
                    else if (IsSubList(output[j], output[i]))
                    {
                        output.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            return output;
        }

        private static bool IsSubList(List<int> list, List<int> subList)
        {
            bool output = true;
            if (subList.Count > list.Count)
                return false;
            for (int i = 0; i < subList.Count; i++)
            {
                if (subList[i] != list[i])
                {
                    output = false;
                    break;
                }
            }
            return output;
        }

        // Hàm này chạy chưa đúng
        private static List<List<int>> ListAllPathsRecursive(Node root, List<int> list)
        {
            List<List<int>> output = new List<List<int>>();
            switch (root.Type)
            {
                case NodeType.Normal:
                    foreach (var item in root.Paths)
                    {
                        output.AddRange(ListAllPathsRecursive(item.Target, list));
                    }
                    break;

                case NodeType.Decision:
                    foreach (var item in root.Paths)
                    {
                        var list2 = new List<int>(list);
                        list2.Add(item.Id);
                        output.AddRange(ListAllPathsRecursive(item.Target, list2));
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