using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UnitTestProjectCollection
{
  public class BlockCollClass
  {
    protected ConcurrentBag<int> bc;

    private void producer()
    {
      for (int i = 0; i < 100; i++)
      {
        bc.Add(i * i);        
        Debug.WriteLine("Create " + i * i);
      }
    }

    private void consumer()
    {
      int i1;
      while(bc.TryTake(out i1))
      {
        
        Debug.WriteLine("Take: " + i1);
      }
    }

    public void Start()
    {
      bc = new ConcurrentBag<int>();

      Task Pr = new Task(producer);
      Task Cn = new Task(consumer);

      Pr.Start();
      Cn.Start();

      try
      {
        Task.WaitAll(Cn, Pr);
      }
      finally
      {
        Cn.Dispose();
        Pr.Dispose();
      }
    }
  }
}
