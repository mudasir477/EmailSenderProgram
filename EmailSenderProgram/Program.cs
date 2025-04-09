using EmailSenderProgram.Emails;
using EmailSenderProgram.Extensions;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Jobs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace EmailSenderProgram
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            SetupBootstrapLogger();

            Log.Information("Starting the application...");

            try
            {
                using (var host = CreateHostBuilder(args).Build())
                {
                    var logger = host.Services.GetRequiredService<ILogger<Program>>();
                    var voucherManager = host.Services.GetRequiredService<IVoucherManager>();
                    var registry = host.Services.GetRequiredService<EmailNotifierRegistry>();
                    var comebackSender = registry.GetNotifier(Constants.EmailNotifier.Comeback);


                    var jobClient = host.Services.GetRequiredService<IBackgroundJobClient>();
                    var server = host.Services.GetRequiredService<BackgroundJobServer>();

                    // Immediately enqueue the jobs for testing
                    jobClient.Enqueue<ComebackEmailJob>(job => job.ExecuteAsync());
                    jobClient.Enqueue<WelcomeEmailJob>(job => job.ExecuteAsync());

                    // Uncomment the following code to schedule the recurring job.
                    //ScheduleEmailJobs();

                    await host.RunAsync();

                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
               .ConfigureSerilog()
               .ConfigureServices((context, services) =>
               {
                   services
                       .ConfigureManagers()
                       .ConfigureEmailNotifierRegistery()
                       .AddInMemoryHangfire()
                       .ConfigureJobs();
               });
        }

        private static void SetupBootstrapLogger()
        {
            var logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            var rollingInterval = ConfigurationManager.AppSettings["RollingInterval"];
            var isDebugMode = bool.TryParse(ConfigurationManager.AppSettings["IsDebugMode"], out bool debugMode) ? debugMode : true;

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File(logFilePath, rollingInterval: (RollingInterval)Enum.Parse(typeof(RollingInterval), rollingInterval), shared: true)
               .CreateBootstrapLogger();
        }
        private static void ScheduleEmailJobs()
        {
            RecurringJob.AddOrUpdate<ComebackEmailJob>(
                       "comeback-email-job",
                       job => job.ExecuteAsync(),
                       Cron.Daily);

            RecurringJob.AddOrUpdate<WelcomeEmailJob>(
                        "welcome-email-job",
                        job => job.ExecuteAsync(),
                        "0 8 * * 0"); // Cron expression for every Sunday at 8:00 AM 
        }
    }
}
