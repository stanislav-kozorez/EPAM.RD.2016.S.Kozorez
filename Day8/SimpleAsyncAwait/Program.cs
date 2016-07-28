﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace SimpleAsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            DoInAsyncWay();
            DoInParallelWay();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void DoInAsyncWay()
        {
            Console.WriteLine("Waiting for task to complete.");

            AsyncHelper.RunSync(DoSomeWork);
            
            // TODO: AsyncHelper.RunSync(DoSomeWork);
            Console.WriteLine("Task is completed. Press any key to continue.");
        }

        private async static Task DoSomeWork()
        {
            Console.WriteLine("Press any key to do the work.");
            Console.ReadKey();

            var stopwatch = new Stopwatch();

            var searchRequest = "https://www.google.by/search?q={0}";
            WebClient webClient = new WebClient();
            stopwatch.Start();
            // TODO: use async/await here to run those tasks in asynchronous style.
            var result1 = await webClient.DownloadStringTaskAsync(string.Format(searchRequest, "pokemon"));
            var result2 = await webClient.DownloadStringTaskAsync(string.Format(searchRequest, "epam"));
            var result3 = await webClient.DownloadStringTaskAsync(string.Format(searchRequest, "minsk"));
            stopwatch.Stop();
            Console.WriteLine(string.Format("Total time is {0}ms.", stopwatch.ElapsedMilliseconds));
        }

        private static void DoInParallelWay()
        {
            Console.WriteLine("Press any key to do the work.");
            Console.ReadKey();

            var stopwatch = new Stopwatch();
            var searchRequest = "https://www.google.by/search?q={0}";
            string[] query = new string[] { "pokemon", "epam", "minsk" };

            var tasks = new List<Task>();

            stopwatch.Start();
            foreach (var q in query)
            {
                // TODO: use factory to create tasks and run them in a parallel style.
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var webClient = new WebClient();
                    webClient.DownloadString(string.Format(searchRequest, q));
                }));
            }

            // TODO: wait here unit all tasks will complete.
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            Console.WriteLine(string.Format("Total time is {0}ms.", stopwatch.ElapsedMilliseconds));
        }
    }
}
