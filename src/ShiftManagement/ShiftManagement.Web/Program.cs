using System;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftManagement.Web.Data;

namespace ShiftManagement.Web
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = GetWebHost(args);

            ExceuteSeeding(host);
            host.Run();
        }

        private static void ExceuteSeeding(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                var seeder = scope.ServiceProvider.GetService<DataSeeder>();
                seeder.SeedAsync().Wait();
                logger.LogInformation("Seeded the database.");
            }
        }

        public static IWebHost GetWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .ConfigureAppConfiguration((hostContext, config) =>
                          {
                              config.Sources.Clear();
                              config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                          }).ConfigureLogging(logging =>
                          {
                              logging.ClearProviders();
                              logging.AddConsole();
                          }).Build();
        }
    }
}
