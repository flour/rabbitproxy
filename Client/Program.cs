using Newtonsoft.Json;
using Shared;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start req/res");

            Run();

            Console.ReadLine();
        }

        static async Task Run()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://localhost:44301/api/logs");
                for (int i = 0; i < 100; i++)
                {
                    var request = new LogMessage { Message = $"log {i}" };
                    var jsonInString = JsonConvert.SerializeObject(request);
                    var response = await client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Request failed {response.StatusCode}");
                    }
                }
            }
        }
    }
}
