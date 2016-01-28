//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Data.SqlClient;
//using System.Data.Entity;
//using FluentScheduler;
//using System.Threading;

//namespace DataStoreClient
//{
//    class Program : Registry
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Starting Up...");
//            TaskManager.Initialize(new TaskScheduler());
//            Console.WriteLine("Initialized");

//            //TaskScheduler _tschedule = new TaskScheduler();
//            //RunBoardAsync().Wait(); //calls RunAsync and block other operation until done.
//        }
//        static async Task RunBoardAsync()
//        {
//            using (var client = new HttpClient())
//            {
//                //Send Http Request
//                client.BaseAddress = new Uri("http://localhost:12314/");
//                client.DefaultRequestHeaders.Accept.Clear();
//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//                //try
//                //{
//                // HTTP GET (Getting resource(s))
//                Console.WriteLine("Now consuming...");
//                HttpResponseMessage response = await client.GetAsync("api/Boards?q={}");
//                response.EnsureSuccessStatusCode();
//                if (response.IsSuccessStatusCode)
//                {
//                    using (var db = new ApplicationContext())
//                    {
//                        Console.WriteLine("Success, Now reading...");
//                        var board = response.Content.ReadAsAsync<dynamic>().Result;
//                        Console.WriteLine("Id\tOwnerId\tTitle\tOrder\tCreatedAt\tLastUPdateTime");
//                        foreach (var x in board.Data)
//                        {
//                            Board b = new Board();
//                            int _emptyID = Convert.ToInt32(x.Id);
//                            var v = db.Boards.Where(c => c.Id.Equals(_emptyID)).FirstOrDefault();
//                            if (v != null)
//                            {                                
//                                v.Id = x.Id;
//                                v.OwnerId = x.OwnerId;
//                                v.Title = x.Title;
//                                v.Order = x.Order;
//                                v.CreatedAt = x.CreatedAt;
//                                v.LastUpdateTime = x.LastUpdateTime;
//                                Console.WriteLine("Updating data number "+ v.Id +"");
//                                db.Entry(v).State = EntityState.Modified;
//                            }
//                            else 
//                            {
//                                b.Id = x.Id;
//                                b.OwnerId = x.OwnerId;
//                                b.Title = x.Title;
//                                b.Order = x.Order;
//                                b.CreatedAt = x.CreatedAt;
//                                b.LastUpdateTime = x.LastUpdateTime;
//                                db.Boards.Add(b);
//                                Console.WriteLine("Adding data number " + b.Id + "");
//                            }
//                            db.SaveChanges();
                            
//                        }
//                    }
//                }
//                else { Console.WriteLine("Unsuccessful"); }
//            }
//        }

//        public void BoardSchedule()
//        {
//            // Schedule a simple task to run at a specific time
//            //Schedule(async () => await RunBoardAsync()).ToRunEvery(1).Days().At(21, 15);

//            // Schedule a more complex action to run immediately and on an monthly interval
//            Schedule(() =>
//            {
//                Console.WriteLine("Complex Action Task Starts: " + DateTime.Now);
//                Thread.Sleep(1000);
//                Console.WriteLine("Complex Action Task Ends: " + DateTime.Now);
//            }).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
            
//        }
        
//    }
//}
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using FluentScheduler.Model;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting everything...");
            TaskManager.Initialize(new MyRegistry());
            Console.WriteLine("Done initializing...");
            Thread.Sleep(10000);
            TaskManager.Stop();

            /*			TaskManager.AddTask(() => Console.WriteLine("Inline task: " + DateTime.Now), x => x.ToRunEvery(15).Seconds());
                        TaskManager.AddTask(() => Console.WriteLine("Inline task (once): " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now.AddSeconds(5)));

                        TaskManager.AddTask<MyInlineTask>(x => x.ToRunNow());
            */
            //TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
            TaskManager.AddTask(() =>
            {
                Console.WriteLine("Inline task: " + DateTime.Now);
                throw new Exception("Hi");
            }, x => x.ToRunNow());

            Console.ReadKey();
        }

        static void TaskManager_UnobservedTaskException(Task sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Something went wrong: " + e.ExceptionObject);
        }
    }

    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            // Schedule an ITask to run at an interval
            Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();

            // Schedule a simple task to run at a specific time
            Schedule(() => Console.WriteLine("Timed Task - Will run every day at 9:15pm: " + DateTime.Now)).ToRunEvery(1).Days().At(21, 15);

            // Schedule a more complex action to run immediately and on an monthly interval
            Schedule(() =>
            {
                Console.WriteLine("Complex Action Task Starts: " + DateTime.Now);
                Thread.Sleep(1000);
                Console.WriteLine("Complex Action Task Ends: " + DateTime.Now);
            }).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
        }
    }

    public class MyTask : ITask
    {
        public void Execute()
        {
            Console.WriteLine("ITask Task: " + DateTime.Now);
        }
    }
    public class MyInlineTask : ITask
    {
        public void Execute()
        {
            Console.WriteLine("ITask Inline Task: " + DateTime.Now);
        }
    }
}