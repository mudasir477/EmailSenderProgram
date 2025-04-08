using EmailSenderProgram.Emails.Base.IManagers;
using EmailSenderProgram.Emails.Base.Models;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSenderProgram.Emails.Base.Managers
{
    public abstract class BaseNotifier : IBaseNotifier
    {
        protected readonly ISmtpManager _emailManager;
        protected readonly AppConfig _configuration;
        protected readonly ITemplateManager _templateManager;

        protected BaseNotifier(
            ISmtpManager emailManager,
            AppConfig configuration,
            ITemplateManager templateManager)
        {
            _emailManager = emailManager;
            _configuration = configuration;
            _templateManager = templateManager;
        }


        public virtual async Task<EmailNotifierResult> SendEmailAsync(IDictionary<string, object> variables)
        {
            Console.WriteLine($"[{GetType().Name}] Starting to send emails...");

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {recipient.Email}. Error: {ex.Message}");
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
