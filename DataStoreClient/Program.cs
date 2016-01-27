using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data.Entity;

namespace DataStoreClient
{
    class Program
    {
        static void Main()
        {
            RunAsync().Wait(); //calls RunAsync and block other operation until done.
        }
        static async Task RunAsync()
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
        
        //static void Connection(string[] args)
        //{
        //    SqlConnection _conn = new SqlConnection("server=SQLEXPRESS;database=DataStore");
        //    SqlCommand _command  = _conn.CreateCommand();

        //    try{
        //        _conn.Open();
        //        _command.CommandText = "CREATE DATABASE DataStore";
        //        Console.WriteLine(_command.CommandText);

        //        _command.ExecuteNonQuery();
        //        Console.WriteLine("Database Created\nNow switching");
        //        _conn.ChangeDatabase("DataStore");

        //        _command.CommandText = "CREATE TABLE (Id integer, OwnerId string, Title string, Order integer)";
        //        Console.WriteLine(_command.CommandText);
        //        Console.WriteLine("Number of Rows affected is: {0}", _command.ExecuteNonQuery());

        //    }
        //    catch(SqlException ex){
        //        Console.WriteLine(ex.ToString());   
        //    }
        //    finally {
        //        _conn.Close();
        //        Console.WriteLine("Conbnection closed");
        //    }

        //}
       
    }
}
