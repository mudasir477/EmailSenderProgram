using EmailSenderProgram.Emails.Base.IManagers;
using EmailSenderProgram.Emails.Base.Models;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace EmailSenderProgram.Emails.Base.Managers
{
    public abstract class BaseNotifier : IBaseNotifier
    {
        protected readonly ISmtpManager _emailManager;
        protected readonly AppConfig _configuration;
        protected readonly ITemplateManager _templateManager;
        protected static readonly ILogger _logger = Log.ForContext<BaseNotifier>();

        protected BaseNotifier(
            ISmtpManager emailManager,
            AppConfig configuration,
            ITemplateManager templateManager)
        {
            _emailManager = Guard.Against.Null(emailManager, nameof(emailManager));
            _configuration = Guard.Against.Null(configuration, nameof(configuration));
            _templateManager = Guard.Against.Null(templateManager, nameof(templateManager));
        }


        public virtual async Task<EmailNotifierResult> SendEmailAsync(IDictionary<string, object> variables)
        {
            _logger.Information("[{NotifierType}] Starting to send emails...", GetType().Name);

            var result = new EmailNotifierResult();
            var recipients = GetRecipients();

            foreach (var recipient in recipients)
            {
                try
                {
                    variables["Email"] = recipient.Email;
                    var body = await GetEmailBody(variables);
                    var subject = GetEmailSubject();

                    await _emailManager.SendAsync(
                        to: recipient.Email,
                        from: _configuration.FromEmail,
                        subject: subject,
                        body: body
                    );

                    result.AddSuccess(recipient);
                    _logger.Information("Email sent successfully to {RecipientEmail}", recipient.Email);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to send email to {recipient.Email}. Error: {ex.Message}");
                    result.AddFailure(recipient);
                }
            }

            return result;
        }
        protected abstract List<Customer> GetRecipients();
        protected abstract string GetEmailSubject();
        protected abstract Task<string> GetEmailBody(IDictionary<string, object> variables);
    }
}
