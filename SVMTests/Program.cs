using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int j = 0;
            int k = 0;

            while (j != 10000000)
            {
                j = j + 1;
                k--;
            }

            j = k;

            Console.WriteLine("TIME: " + sw.ElapsedMilliseconds);
        }
    }
}
