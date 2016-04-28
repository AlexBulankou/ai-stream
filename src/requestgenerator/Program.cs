using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace requestgenerator
{
    class Program
    {
        private static string url;
        private static int intervalPerThread;
        private static bool stopRequests = false;

        static void Main(string[] args)
        {
            Program.url = args[0];
            double rps = double.Parse(args[1]);
            int numThreads = Environment.ProcessorCount * 3;
            Program.intervalPerThread = (int)(1000.00 / rps * (double)numThreads);

            List<Task> tasks = new List<Task>();
           
            for (var i = 0; i < numThreads; i++)
            {
                var task = Task.Factory.StartNew(GenerateRequests, TaskCreationOptions.LongRunning);
                tasks.Add(task);
                Thread.Sleep((int)(1000.00 / (double)rps));
            }

            Console.ReadLine();
            stopRequests = true;
            Task.WaitAll(tasks.ToArray());
        }

        static void GenerateRequests()
        {
            Stopwatch watch = new Stopwatch();
            int timeLeftToWait = 0;
            while (!stopRequests)
            {
                watch.Reset();
                watch.Start();
                HttpClient httpClient = new HttpClient();
                httpClient.GetAsync(Program.url).Wait();
                Console.Write(" "+timeLeftToWait.ToString()+" ");
                timeLeftToWait = (int)(intervalPerThread - watch.ElapsedMilliseconds);
                Thread.Sleep(Math.Max(0, timeLeftToWait));

            }
        }

    }
}
