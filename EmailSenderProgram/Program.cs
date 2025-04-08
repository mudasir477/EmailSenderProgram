using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSenderProgram.Emails;
using EmailSenderProgram.Extensions;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailSenderProgram
{
    internal class Program
    {
        public static  async Task Main(string[] args)
        {

            using (var host = CreateHostBuilder(args).Build())
            {
               
                var registry = host.Services.GetRequiredService<EmailNotifierRegistry>();
                var comebackSender = registry.GetNotifier(Constants.EmailNotifier.Comeback);

                var variables = new Dictionary<string, object>
                {
                    { "VoucherCode", "EOComebackDiscount" },
                };

                var comebackResult = await comebackSender.SendEmailAsync(variables);
                Console.WriteLine($"Comeback email sent successfully? {comebackResult.IsSuccess()}");

               
                if (DateTimeProvider.IsSunday())
                {
                    var welcomeSender = registry.GetNotifier(Constants.EmailNotifier.Welcome);
                    var welcomeResult = await welcomeSender.SendEmailAsync(new Dictionary<string, object> { });
                    Console.WriteLine($"Welcome email sent successfully? {welcomeResult.IsSuccess()}");
                }

                Console.ReadKey();
            }

        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
               {
                   services
                       .ConfigureManagers()                       
                       .ConfigureEmailNotifierRegistery();
               });
    }
}
