using Ardalis.GuardClauses;
using EmailSenderProgram.Emails;
using EmailSenderProgram.Extensions.Services;

using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSenderProgram.Jobs
{
    public class WelcomeEmailJob
    {
        private readonly EmailNotifierRegistry _notifierRegistry;
        protected static readonly ILogger _logger = Log.ForContext<ComebackEmailJob>();

        public WelcomeEmailJob(
            EmailNotifierRegistry notifierRegistry
           )
        {
           
            _notifierRegistry = Guard.Against.Null(notifierRegistry, nameof(notifierRegistry));
        }

        public async Task ExecuteAsync()
        {

            var welcomeSender = _notifierRegistry.GetNotifier(Constants.EmailNotifier.Welcome);
            var welcomeResult = await welcomeSender.SendEmailAsync(new Dictionary<string, object>());
            _logger.Information($"Welcome email sent successfully? {welcomeResult.IsSuccess()}");

        }
    }
}