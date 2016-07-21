using System;
using System.Collections;

namespace TaskConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      int time = 20;

      if (time > MyClassLibrary.Helpers.WaitTime)
      {
        Console.WriteLine("time : {0} > Wait Time {1}", time, MyClassLibrary.Helpers.WaitTime);
      }
      else
      {
        Console.WriteLine("time : {0} <= Wait Time {1}", time, MyClassLibrary.Helpers.WaitTime);
      }

      Console.WriteLine("========================================================");

      var result = MyClassLibrary.GetResult.GetUserResult("Red");

      var work = new Work();
      work.DoSomething(result);

            Hashtable table = new Hashtable();
            table.Add(1, new object());
            table.Add(3, "sdsdf");
            table.Add(5, 2);

            Console.WriteLine(table[1].GetType());
            Console.WriteLine(table[3].GetType());
            Console.WriteLine(table[5].GetType());


            Console.ReadLine();
    }
  }
}
