using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

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
                    Console.WriteLine("Success, Now reading...");
                    var board = response.Content.ReadAsAsync<dynamic>().Result;
                    foreach (var x in board.Data)
                    {
                        //Console.WriteLine("{0}\t{1}", x.Data.Id, x.Data.OwnerId);
                    }
                }
                else { Console.WriteLine("Unsuccessful"); }
                
                

                #region example, dont delete it until done
                
                #endregion
                #region
                #endregion




            }
        }
       
    }
}
