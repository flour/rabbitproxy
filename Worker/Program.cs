using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Worker
{
    class Program
    {
        public static void Main(string[] args)
        {
            var rabbitHost = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? "localhost";
            var msgBus = EasyNetQ.RabbitHutch.CreateBus($"host={rabbitHost};timeout=30");
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // services.

                    var workers = new BlockingCollection<Worker>();
                    for (int i = 0; i < 5; i++)
                    {
                        workers.TryAdd(new Worker($"Worker #{i + 1}"));
                    }
                    msgBus.RespondAsync<Shared.IMessage, Shared.IMessage>(request =>
                        Task.Run(() =>
                        {
                            var worker = workers.Take();
                            try
                            {
                                return worker.HandleMessage(request);
                            }
                            finally
                            {
                                workers.Add(worker);
                            }
                        })
                    );
                });

            hostBuilder.RunConsoleAsync().Wait();
            msgBus.Dispose();
            Debug.WriteLine("I'm out! Enough of this shit!!");
        }
    }
}
