using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new ManualResetEventSlim(false);
            var pingEvent = new AutoResetEvent(true);
            var pongEvent = new AutoResetEvent(false);

            CancellationTokenSource cts = new CancellationTokenSource(); // TODO: Create a new cancellation token source.
            CancellationToken token = cts.Token; // TODO: Assign an appropriate value to token variable.

            Action ping = () =>
            {
                Console.WriteLine("ping: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {
                    // TODO: write ping-pong functionality here using pingEvent and pongEvent here.
                    pingEvent.WaitOne();
                    Console.WriteLine("ping!");

                    Thread.Sleep(1000);
                    pingEvent.Reset();
                    pongEvent.Set();
                    continueRunning = !token.IsCancellationRequested; // TODO: Use cancellation token "token" internals here to set appropriate value.
                }

                // TODO: Fix issue with blocked pong task.

                Console.WriteLine("ping: done");
            };

            Action pong = () =>
            {
                Console.WriteLine("pong: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {
                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pongEvent.WaitOne();
                    Console.WriteLine("pong!");

                    Thread.Sleep(1000);

                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pongEvent.Reset();
                    pingEvent.Set();
                    
                    continueRunning = !token.IsCancellationRequested; // TODO: Use cancellation token "token" internals here to set appropriate value.
                }

                // TODO: Fix issue with blocked ping task.

                Console.WriteLine("pong: done");
            };


            var pingTask = Task.Run(ping);
            var pongTask = Task.Run(pong);

            Console.WriteLine("Press any key to start.");
            Console.WriteLine("After ping-pong game started, press any key to exit.");
            Console.ReadKey();

            start.Set();

            Console.ReadKey();
            // TODO: cancel both tasks using cancellation token.
            cts.Cancel();
            pingTask.Wait();
            pongTask.Wait();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
