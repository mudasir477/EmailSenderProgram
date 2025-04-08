using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderProgram.Emails
{
    public static class Constants
    {
        public static class EmailNotifier
        {
            public const string Comeback = @"Comeback";
            public const string Welcome = @"Welcome";
        }
        public static class EmailTemplates
        {
            public const string ComebackEmail = @"Comeback/ComebackNotifier.html";
            public const string WelcomeEmail = @"Welcome/WelcomeNotifier.html";
        }


    }
}
