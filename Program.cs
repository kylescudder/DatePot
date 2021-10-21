using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace DatePot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string version = configuration.GetSection("Version")["Major"] + "." + configuration.GetSection("Version")["Minor"] + "." + configuration.GetSection("Version")["Revision"];

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Sentry(o =>
            {
                o.Release = "the-date-pot@" + version;
                o.Dsn = "https://8f7807e0fb4d48d2ac11d1b3cec2412a@o1044877.ingest.sentry.io/6020109";
                // Debug and higher are stored as breadcrumbs (default is Information)
                o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                // Warning and higher is sent as event (default is Error)
                o.MinimumEventLevel = LogEventLevel.Information;
                o.TracesSampleRate = 1.0;
            })
            .CreateLogger();

            try
            {
                Log.Information("Appliation Starting Up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
