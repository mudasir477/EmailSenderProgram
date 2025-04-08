using System;

namespace EmailSenderProgram.Emails.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EmailNotifierAttribute : Attribute
    {
        public string Key { get; }

        public EmailNotifierAttribute(string key)
        {
            Key = key;
        }
    }
}

