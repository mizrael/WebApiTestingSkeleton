using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApiTestingSkeleton.Core.Web
{
    public class WebHostBuilderFactory
    {
        public static IWebHostBuilder Build<TStartup>(string[] args)
            where TStartup : class
        {
            var currDir = Directory.GetCurrentDirectory();

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(currDir)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;

                    builder.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<TStartup>();
        }
    }
}