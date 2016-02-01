using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using FluentScheduler;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.Entity;

namespace DataStoreClient
{
    public class TaskScheduler: Registry //ITask, IRegisteredObject
    {
        
        private readonly object _lock = new object();
        private bool _shuttingDown;
        public TaskScheduler()
        {
            Schedule<BoardFetch>().ToRunNow().AndEvery(60).Seconds();
            // Register this task with hosting evirontment 
            // allow for more gracefull stop the task, in case IIS shutting down

        }
        
    }
    public class BoardFetch : ITask
    {
        public void Execute()
        {
            Console.WriteLine("ITask Task: " + DateTime.Now);
            using (var client = new HttpClient())
            {
                //Send Http Request
                client.BaseAddress = new Uri("http://localhost:12314/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //try
                //{
                // HTTP GET (Getting resource(s))
                Console.WriteLine("Now consuming board data...");
                HttpResponseMessage response = client.GetAsync("api/Boards?q={}").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    using (var db = new ApplicationContext())
                    {
                        Console.WriteLine("Success, Now reading board data...");
                        var board = response.Content.ReadAsAsync<dynamic>().Result;
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
                                Console.WriteLine("Updating data number " + v.Id + "");
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
        //static async Task RunBoardAsync()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //Send Http Request
        //        client.BaseAddress = new Uri("http://localhost:12314/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //try
        //        //{
        //        // HTTP GET (Getting resource(s))
        //        Console.WriteLine("Now consuming...");
        //        HttpResponseMessage response = await client.GetAsync("api/Boards?q={}");
        //        response.EnsureSuccessStatusCode();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            using (var db = new ApplicationContext())
        //            {
        //                Console.WriteLine("Success, Now reading...");
        //                var board = response.Content.ReadAsAsync<dynamic>().Result;
        //                Console.WriteLine("Id\tOwnerId\tTitle\tOrder\tCreatedAt\tLastUPdateTime");
        //                foreach (var x in board.Data)
        //                {
        //                    Board b = new Board();
        //                    int _emptyID = Convert.ToInt32(x.Id);
        //                    var v = db.Boards.Where(c => c.Id.Equals(_emptyID)).FirstOrDefault();
        //                    if (v != null)
        //                    {
        //                        v.Id = x.Id;
        //                        v.OwnerId = x.OwnerId;
        //                        v.Title = x.Title;
        //                        v.Order = x.Order;
        //                        v.CreatedAt = x.CreatedAt;
        //                        v.LastUpdateTime = x.LastUpdateTime;
        //                        Console.WriteLine("Updating data number " + v.Id + "");
        //                        db.Entry(v).State = EntityState.Modified;
        //                    }
        //                    else
        //                    {
        //                        b.Id = x.Id;
        //                        b.OwnerId = x.OwnerId;
        //                        b.Title = x.Title;
        //                        b.Order = x.Order;
        //                        b.CreatedAt = x.CreatedAt;
        //                        b.LastUpdateTime = x.LastUpdateTime;
        //                        db.Boards.Add(b);
        //                        Console.WriteLine("Adding data number " + b.Id + "");
        //                    }
        //                    db.SaveChanges();

        //                }
        //            }
        //        }
        //        else { Console.WriteLine("Unsuccessful"); }
        //    }
        //}
    }
    public class InlineTask : ITask
    {
        public void Execute()
        {
            Console.WriteLine("Inline Task: " + DateTime.Now);

        }
    }
}
