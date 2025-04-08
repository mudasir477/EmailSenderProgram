using EmailSenderProgram.Emails.Base.Attributes;
using EmailSenderProgram.Emails.Base.Managers;
using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSenderProgram.Emails.Comeback
{

    [EmailNotifier(Constants.EmailNotifier.Comeback)]
    public class ComebackNotifier : BaseNotifier
    {
        private string _voucherCode;
        public ComebackNotifier(
            ISmtpManager emailManager,
            AppConfig configuration,
            ITemplateManager templateManager
        ) : base(emailManager, configuration, templateManager)
        {
        }
        protected override List<Customer> GetRecipients()
        {
            var orders = DataLayer.ListOrders();
            var customers = DataLayer.ListCustomers();

            return customers
                .Where(c => !orders.Any(o => o.CustomerEmail == c.Email))
                .ToList();
        }

        protected override async Task<string> GetEmailBody(IDictionary<string, object> variables)
        {
            return await _templateManager.RenderTemplate(Constants.EmailTemplates.ComebackEmail, variables);
        }

        protected override string GetEmailSubject()
        {
            return "We miss you as a customer!";
        }
    }
}
