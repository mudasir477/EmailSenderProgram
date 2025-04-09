using Ardalis.GuardClauses;
using EmailSenderProgram.Emails;
using EmailSenderProgram.Extensions.Services;
using EmailSenderProgram.Infrastructure.IManagers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSenderProgram.Jobs
{
    public class ComebackEmailJob
    {
        private readonly IVoucherManager _voucherManager;
        private readonly EmailNotifierRegistry _notifierRegistry;
        protected static readonly ILogger _logger = Log.ForContext<ComebackEmailJob>();

        public ComebackEmailJob(
            IVoucherManager voucherManager,
            EmailNotifierRegistry notifierRegistry
        )
        {
            _voucherManager = Guard.Against.Null(voucherManager, nameof(voucherManager));
            _notifierRegistry = Guard.Against.Null(notifierRegistry, nameof(notifierRegistry));
            
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var comebackSender = _notifierRegistry.GetNotifier(Constants.EmailNotifier.Comeback);

                var variables = new Dictionary<string, object>
                {
                    { "VoucherCode", _voucherManager.GetVoucherCode() }
                };

                var comebackResult = await comebackSender.SendEmailAsync(variables);
                _logger.Information($"[ComebackEmailJob] Email sent successfully? {comebackResult.IsSuccess()}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[ComebackEmailJob] Failed to send comeback email.");
            }
        }
    }
}
