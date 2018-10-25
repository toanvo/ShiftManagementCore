using System;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftManagement.Web.Data;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using NLog;
using LogLevel = NLog.LogLevel;

namespace ShiftManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var host = GetWebHost(args);

                ExceuteSeeding(host);
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
            finally
            {
                LogManager.Shutdown();
            }
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
                              logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                              logging.AddConsole();
                          }).UseNLog()
                          .Build();
        }
    }
}
