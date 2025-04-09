using EmailSenderProgram.Emails.Base.Attributes;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Infrastructure;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Infrastructure.Managers;
using EmailSenderProgram.Jobs;
using EmailSenderProgram.Models;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Reflection;


namespace EmailSenderProgram.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection ConfigureManagers(this IServiceCollection services)
        {
            AppConfig config = AppConfig.GetInstance();
            services.AddSingleton(config);
            services.AddTransient<IVoucherManager, VoucherManager>();
            services.AddTransient<ISmtpManager, SmtpManager>();
            services.AddTransient<ITemplateManager>(sp =>
                new TemplateManager(config.TemplatesFolderName));

            return services;
        }
        public static IServiceCollection ConfigureJobs(this IServiceCollection services)
        {

            services.AddTransient<ComebackEmailJob>();
            services.AddTransient<WelcomeEmailJob>();
            return services;
        }
        public static IServiceCollection ConfigureEmailNotifierRegistery(this IServiceCollection services)
        {
            services.Scan(scan => scan
                        .FromAssemblies(Assembly.GetExecutingAssembly())
                        .AddClasses(classes => classes.WithAttribute<EmailNotifierAttribute>())
                        .AsSelf()
                        .WithTransientLifetime());

            services.AddSingleton<EmailNotifierRegistry>(sp =>
            {
                var registry = new EmailNotifierRegistry(sp);

                registry.AutoRegisterNotifiers(Assembly.GetExecutingAssembly());
                return registry;
            });
            return services;
        }
        public static IServiceCollection ConfigureSerilog(this IServiceCollection services)
        {
            var logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            var rollingInterval = ConfigurationManager.AppSettings["RollingInterval"];
            var isDebugMode = bool.TryParse(ConfigurationManager.AppSettings["IsDebugMode"], out bool debugMode) ? debugMode : true;
            return services;
        }
        public static IServiceCollection AddInMemoryHangfire(this IServiceCollection services)
        {
            services.AddSingleton<JobStorage>(provider =>
            {
                var memoryStorage = new MemoryStorage();
                GlobalConfiguration.Configuration
                .UseStorage(memoryStorage)
                .UseActivator(new HangfireActivator(provider)); 
                return memoryStorage;
            });
            services.AddSingleton<IBackgroundJobClient>(provider =>
            new BackgroundJobClient(provider.GetRequiredService<JobStorage>()));
            services.AddSingleton<BackgroundJobServer>();
            return services;
        }

    }
}
