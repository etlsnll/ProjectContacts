using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using ProjectContacts.Repository;

namespace ProjectContacts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: Setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("initialize Program.Main()");
                var host = BuildWebHost(args);

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<ProjectContactsContext>();
                        context.Database.Migrate();
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        //var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.Error(ex, "An error occurred seeding the DB.");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                // Catch setup errors
                logger.Error(ex, "Error with BuildWebHost()");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()  // NLog: setup NLog for Dependency injection
                .Build();
    }
}
