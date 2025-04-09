using EmailSenderProgram.Emails;
using EmailSenderProgram.Extensions;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Infrastructure.IManagers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
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

                    var variables = new Dictionary<string, object>
                    {
                        { "VoucherCode", voucherManager.GetVoucherCode() },
                    };

                    var comebackResult = await comebackSender.SendEmailAsync(variables);
                    logger.LogInformation($"Comeback email sent successfully? {comebackResult.IsSuccess()}");


                    if (DateTimeProvider.IsSunday())
                    {
                        var welcomeSender = registry.GetNotifier(Constants.EmailNotifier.Welcome);
                        var welcomeResult = await welcomeSender.SendEmailAsync(new Dictionary<string, object> { });
                        logger.LogInformation($"Welcome email sent successfully? {welcomeResult.IsSuccess()}");
                    }

                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensures logs are flushed to file and console
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
                       .ConfigureEmailNotifierRegistery();
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
    }
}
