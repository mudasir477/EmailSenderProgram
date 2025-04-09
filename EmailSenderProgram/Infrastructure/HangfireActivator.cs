using Hangfire;
using System;

namespace EmailSenderProgram.Infrastructure
{
    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;
        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetService(jobType)
            ?? Activator.CreateInstance(jobType);
        }
    }
}
