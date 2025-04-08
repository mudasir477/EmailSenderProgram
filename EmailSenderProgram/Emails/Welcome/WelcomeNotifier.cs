using EmailSenderProgram.Emails.Base.Attributes;
using EmailSenderProgram.Emails.Base.Managers;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSenderProgram.Emails.Welcome
{
    [EmailNotifier(Constants.EmailNotifier.Welcome)]
    public class WelcomeNotifier : BaseNotifier
    {
        public WelcomeNotifier(
            ISmtpManager emailManager,
            AppConfig configuration,
            ITemplateManager templateManager
        ) : base(emailManager, configuration, templateManager)
        {
        }

        protected override List<Customer> GetRecipients()
        {

            return DataLayer.ListCustomers()
                .Where(c => c.CreatedDateTime.Date == DateTime.Now.AddDays(-1).Date)
                .ToList();
        }

        protected override async Task<string> GetEmailBody(IDictionary<string, object> variables)
        {
            return await _templateManager.RenderTemplate(Constants.EmailTemplates.WelcomeEmail, variables);
        }

        protected override string GetEmailSubject()
        {
            return "Welcome to our store!";
        }
    }
}
