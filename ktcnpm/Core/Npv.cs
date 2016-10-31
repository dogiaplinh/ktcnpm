using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
