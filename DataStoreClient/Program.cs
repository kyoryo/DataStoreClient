using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.SqlClient;

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
                #region test response single
                //HttpResponseMessage response = await client.GetAsync("api/boards/1");
                //if (response.IsSuccessStatusCode)
                //{
                //    Console.WriteLine("Success, Now reading...");
                //    Board board = await response.Content.ReadAsAsync<Board>();
                //    Console.WriteLine("");
                //    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                //        board.Id, board.OwnerId, board.Title, board.Order, board.CreatedAt, board.LastUpdateTime, board.IsDeleted);
                //}
                //else { Console.WriteLine("Unsuccessful\n" + response); }
                #endregion
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
                            //Console.WriteLine(x.Id);
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                            x.Id, x.OwnerId, x.Title, x.Order, x.CreatedAt, x.LastUpdateTime);
                        }
                    }
                    
                }
                else { Console.WriteLine("Unsuccessful"); }
            }
        }
        static void Connection(string[] args)
        {
            SqlConnection _conn = new SqlConnection("server=SQLEXPRESS;database=DataStore");
            SqlCommand _command  = _conn.CreateCommand();

            try{
                _conn.Open();
                _command.CommandText = "CREATE DATABASE DataStore";
                Console.WriteLine(_command.CommandText);

                _command.ExecuteNonQuery();
                Console.WriteLine("Database Created\nNow switching");
                _conn.ChangeDatabase("DataStore");

                _command.CommandText = "CREATE TABLE (Id integer, OwnerId string, Title string, Order integer)";
                Console.WriteLine(_command.CommandText);
                Console.WriteLine("Number of Rows affected is: {0}", _command.ExecuteNonQuery());

            }
            catch(SqlException ex){
                Console.WriteLine(ex.ToString());   
            }
            finally {
                _conn.Close();
                Console.WriteLine("Conbnection closed");
            }

        }
       
    }
}
