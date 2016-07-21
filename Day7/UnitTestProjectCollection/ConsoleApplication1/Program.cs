using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        delegate void My(int num);
        static void Main(string[] args)
        {
            Expression<My> exp = x => Console.WriteLine("number = {0}", x);
            exp.Compile()(7);
            Expression<Func<int, bool>> lambda = x => x < 5;
            Console.WriteLine(lambda);
            var result = lambda.Compile();
            Console.WriteLine(result(7));
            Console.ReadKey();
        }
    }
}
