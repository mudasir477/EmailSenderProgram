using EmailSenderProgram.Emails.Base.Attributes;
using EmailSenderProgram.Emails.Base.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EmailSenderProgram.Extensions.Services
{
    public class EmailNotifierRegistry
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _senderTypes;

        public EmailNotifierRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _senderTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }


        public void RegisterNotifier(string key, Type senderType)
        {
            _senderTypes[key] = senderType;
        }


        public void AutoRegisterNotifiers(Assembly assembly)
        {
            var senderTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                            typeof(BaseNotifier).IsAssignableFrom(t) &&
                            t.GetCustomAttribute<EmailNotifierAttribute>() != null);

            foreach (var type in senderTypes)
            {
                var attr = type.GetCustomAttribute<EmailNotifierAttribute>();
                if (!_senderTypes.ContainsKey(attr.Key))
                {
                    _senderTypes.Add(attr.Key, type);
                }
            }
        }


        public BaseNotifier GetNotifier(string key)
        {
            if (_senderTypes.TryGetValue(key, out var type))
            {
                return (BaseNotifier)_serviceProvider.GetRequiredService(type);
            }

            throw new KeyNotFoundException($"No email sender registered for key '{key}'.");
        }
    }
}
