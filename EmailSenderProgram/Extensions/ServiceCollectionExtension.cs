using EmailSenderProgram.Emails.Base.Attributes;
using EmailSenderProgram.Emails.Base.IManagers;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Infrastructure.Managers;
using EmailSenderProgram.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmailSenderProgram.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection ConfigureManagers(this IServiceCollection services)
        {
            AppConfig config = AppConfig.GetInstance();
            services.AddSingleton(config);

            services.AddTransient<ISmtpManager, SmtpManager>();
            services.AddTransient<ITemplateManager>(sp =>
                new TemplateManger(config.TemplatesFolderName));

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
    }
}
