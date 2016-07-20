using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        delegate void Del(int first);

        static void Main(string[] args)
        {
            Del d1 = x => Console.Write("0");
            Del d2 = x => Console.Write("0");

            Console.WriteLine($"First {d1.GetHashCode()} Second {d2.GetHashCode()}");

            var kvpFirst = new KeyValuePair<int, string>(10, "30");
            var kvpSecond = new KeyValuePair<int, string>(10, "10");

            Console.WriteLine($"First {kvpFirst.GetHashCode()} Second {kvpSecond.GetHashCode()}");

            var s1 = "test";
            var s2 = "test";

            var sb = new StringBuilder();
            var s = "";

            var timer = new Stopwatch();

            timer.Start();
            for(int i = 0; i < 10000; i++)
            {
                sb.Append("t");

            }
            timer.Stop();
            timer.Reset();
            Console.WriteLine(timer.ElapsedMilliseconds);

            timer.Start();
            for (int i = 0; i < 10000; i++)
            {
                s += "t";

            }
            timer.Stop();

            Console.WriteLine(timer.ElapsedMilliseconds);

            Console.WriteLine(Object.Equals(s1, s2));

            Console.ReadKey();
        }
    }
}
