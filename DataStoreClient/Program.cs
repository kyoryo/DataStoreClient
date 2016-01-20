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

                try
                {
                    // HTTP GET (Getting resource(s))
                    HttpResponseMessage response = await client.GetAsync("api/boards/1");
                    if (response.IsSuccessStatusCode)
                    {
                        Board board = await response.Content.ReadAsAsync<Board>();
                        Console.WriteLine("{0}\t${1}\t{2}",
                            board.Id, board.OwnerId, board.Title, board.Order, board.CreatedAt, board.LastUpdateTime, board.IsDeleted);
                    }
                }
                catch (HttpRequestException ex)
                {

                }
                
                //HTTP POST
                /////
                /////test variable
                /////

                

            }
        }
    }
}
