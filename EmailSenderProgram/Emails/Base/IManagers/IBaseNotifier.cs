using EmailSenderProgram.Emails.Base.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSenderProgram.Emails.Base.IManagers
{
    public interface IBaseNotifier
    {
        Task<EmailNotifierResult> SendEmailAsync(IDictionary<string, object> variables);
    }
}
