using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] costs = new double[] { 250000 };
            double[] revenues = new double[] { 0, 70000, 70000, 70000, 70000, 70000 };
            double rate;
            for (int i = 0; i < 20; i++)
            {
                rate = i + 1;
                Console.WriteLine($"Rate = {rate:##}\tNPV = {Core.Npv.CalculateNpv(costs, revenues, rate)}");
            }
        }
    }
}
