using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Sentry;

namespace DatePot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
			string version = configuration.GetSection("Version")["Major"] + "." + configuration.GetSection("Version")["Minor"] + "." + configuration.GetSection("Version")["Revision"];
			using (SentrySdk.Init(o =>
            {
                o.Release = "the-date-pot@" + version;
                o.Dsn = "https://8f7807e0fb4d48d2ac11d1b3cec2412a@o1044877.ingest.sentry.io/6020109";
                // When configuring for the first time, to see what the SDK is doing:
                o.Debug = true;
                // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
            }))
			{
				// App code goes here. Dispose the SDK before exiting to flush events.

				try
				{
					SentrySdk.CaptureMessage("Appliation Starting Up");
					CreateHostBuilder(args).Build().Run();
				}
				catch (Exception ex)
				{
					SentrySdk.CaptureException(ex);
				}
				finally
				{
					Log.CloseAndFlush();
				}
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
