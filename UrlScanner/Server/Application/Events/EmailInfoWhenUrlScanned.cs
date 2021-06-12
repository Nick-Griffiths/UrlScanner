using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Email;
using UrlScanner.Server.Infrastructure.Events;

namespace UrlScanner.Server.Application.Events
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class EmailInfoWhenUrlScanned : IEventHandler<UrlScanned>
    {
        private readonly IEmailService _email;
        private readonly UrlScanningContext _db;
        private readonly EmailOptions _options;
        private readonly ILogger<EmailInfoWhenUrlScanned> _logger;

        public EmailInfoWhenUrlScanned(
            IEmailService email,
            UrlScanningContext db,
            IOptionsSnapshot<EmailOptions> options,
            ILogger<EmailInfoWhenUrlScanned> logger)
        {
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
        }

        public async Task Handle(UrlScanned @event)
        {
            _logger.LogInformation($"Emailing scan results for URL with ID: {@event.UrlId} to {_options.ToAddress}");

            var info = await _db.UrlInfos.FindAsync(@event.UrlId);
            var subject = $"{info.Url} has been scanned";
            var builder = new StringBuilder();
            builder.AppendLine($"Scan Results for {info.Url}");
            builder.AppendLine($"Has Google: {(@event.HasGoogle ? "Yes" : "No")}");
            builder.AppendLine($"Phone Numbers: {@event.PhoneNumbers}");
            builder.AppendLine($"Scan Duration: {@event.ScanDuration.TotalMilliseconds:F0}");

            await _email.Send(_options.FromAddress, _options.ToAddress, subject, builder.ToString());
        }
    }
}