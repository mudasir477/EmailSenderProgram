using Microsoft.Extensions.Hosting;
using Serilog;
using System;


namespace EmailSenderProgram.Extensions
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                var logFilePath = context.Configuration["LogFilePath"] ?? "logs\\app-log.txt";
                var rollingInterval = context.Configuration["RollingInterval"] ?? "Day";

                loggerConfiguration
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File(
                        logFilePath,
                        rollingInterval: (RollingInterval)Enum.Parse(typeof(RollingInterval), rollingInterval),
                        shared: true
                    );
            });
        }
    }
}
