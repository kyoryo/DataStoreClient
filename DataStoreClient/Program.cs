using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data.Entity;
using FluentScheduler;
using System.Threading;

namespace DataStoreClient
{
    class Program : Registry
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Up...");
            TaskManager.Initialize(new TaskScheduler());
            Console.WriteLine("Initialized");

            TaskManager.AddTask(() =>
            {
                Console.WriteLine("Inline task: " + DateTime.Now);
            }, x => x.ToRunNow());
            //TaskScheduler _tschedule = new TaskScheduler();
            //RunBoardAsync().Wait(); //calls RunAsync and block other operation until done.
            Console.ReadKey();
        }

        public void BoardSchedule()
        {
            // Schedule a simple task to run at a specific time
            //Schedule(async () => await RunBoardAsync()).ToRunEvery(1).Days().At(21, 15);

            // Schedule a more complex action to run immediately and on an monthly interval
            Schedule(() =>
            {
                Console.WriteLine("Complex Action Task Starts: " + DateTime.Now);
                Thread.Sleep(1000);
                Console.WriteLine("Complex Action Task Ends: " + DateTime.Now);
            }).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

        }

    }
}
