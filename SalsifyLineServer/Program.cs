using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalsifyLineClient;
using System.Collections.Generic;

namespace SalsifyLineServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:5000")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCommandLine(args, new Dictionary<string, string> {
                        {"-file", "fname" }
                    });
                })
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}