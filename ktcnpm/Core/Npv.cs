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
        public static List<RouteItem> ListAllRoutes(Node root)
        {
            RouteHelper helper = new RouteHelper(root);
            return helper.ListAllRoutes();
        }

        public static double CalculateNpv(Node node, RouteItem route)
        {
            return 0;
        }
    }
}