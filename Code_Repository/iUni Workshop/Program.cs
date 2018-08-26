using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data.Seeds;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iUni_Workshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
           
            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
//                    UserSeed.Initialize(services, logger).Wait();
//                    StateSeed.Initialize(services);
                }
                catch (Exception ex) {
                    services.GetRequiredService<ILogger<Program>>().LogError(ex, "An error occurred while seeding the database");
                    throw;
                }
            }
            host.Run();
//            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
