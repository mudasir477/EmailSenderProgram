using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSenderProgram.Infrastructure.IManagers
{
    public interface ITemplateManager
    {
        Task<string> RenderTemplate(string templateName, IDictionary<string, object> variables);
    }

}
