using EmailSenderProgram.Infrastructure.IManagers;
using EmailSenderProgram.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSenderProgram.Infrastructure.Managers
{

    public class SmtpManager : ISmtpManager
    {
        private readonly AppConfig _configuration;

        public SmtpManager(AppConfig configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SendAsync(string to, string from, string subject, string body)
        {
            if (_configuration.IsDebugMode)
            {
                Console.WriteLine($"[Debug Mode] Would send mail to: {to}");
            }
            else
            {
                try
                {
                    using (var smtpClient = new SmtpClient(_configuration.EmailHost))
                    {
                        smtpClient.Port = _configuration.Port;
                        smtpClient.Credentials = new NetworkCredential(_configuration.UserName, _configuration.Password);

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(from),
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        };
                        mailMessage.To.Add(to);

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {to}: {ex.Message}");
                }
            }
        }
    }
}
