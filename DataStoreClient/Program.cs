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
        static void Main()
        {
            //TaskScheduler _tschedule = new TaskScheduler();
            //RunBoardAsync().Wait(); //calls RunAsync and block other operation until done.
        }
        static async Task RunBoardAsync()
        {
            using (var client = new HttpClient())
            {
                //Send Http Request
                client.BaseAddress = new Uri("http://localhost:12314/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //try
                //{
                // HTTP GET (Getting resource(s))
                Console.WriteLine("Now consuming...");
                HttpResponseMessage response = await client.GetAsync("api/Boards?q={}");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    using (var db = new ApplicationContext())
                    {
                        Console.WriteLine("Success, Now reading...");
                        var board = response.Content.ReadAsAsync<dynamic>().Result;
                        Console.WriteLine("Id\tOwnerId\tTitle\tOrder\tCreatedAt\tLastUPdateTime");
                        foreach (var x in board.Data)
                        {
                            Board b = new Board();
                            int _emptyID = Convert.ToInt32(x.Id);
                            var v = db.Boards.Where(c => c.Id.Equals(_emptyID)).FirstOrDefault();
                            if (v != null)
                            {                                
                                v.Id = x.Id;
                                v.OwnerId = x.OwnerId;
                                v.Title = x.Title;
                                v.Order = x.Order;
                                v.CreatedAt = x.CreatedAt;
                                v.LastUpdateTime = x.LastUpdateTime;
                                Console.WriteLine("Updating data number "+ v.Id +"");
                                db.Entry(v).State = EntityState.Modified;
                            }
                            else 
                            {
                                b.Id = x.Id;
                                b.OwnerId = x.OwnerId;
                                b.Title = x.Title;
                                b.Order = x.Order;
                                b.CreatedAt = x.CreatedAt;
                                b.LastUpdateTime = x.LastUpdateTime;
                                db.Boards.Add(b);
                                Console.WriteLine("Adding data number " + b.Id + "");
                            }
                            db.SaveChanges();
                            
                        }
                    }
                }
                else { Console.WriteLine("Unsuccessful"); }
            }
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
