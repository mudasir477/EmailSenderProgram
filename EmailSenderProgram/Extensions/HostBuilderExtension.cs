using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;


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
                        shared:true
                    );
            });
        }
    }
}
