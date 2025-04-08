using System.Threading.Tasks;

namespace EmailSenderProgram.Infrastructure.IManagers
{
    public interface ISmtpManager
    {
        Task SendAsync(string to, string from, string subject, string body);
    }
}
