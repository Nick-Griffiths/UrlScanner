using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Events;

namespace UrlScanner.Server.Application.Events
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class SaveResultsWhenUrlScanned : IEventHandler<UrlScanned>
    {
        private readonly UrlScanningContext _db;
        private readonly ILogger<SaveResultsWhenUrlScanned> _logger;

        internal SaveResultsWhenUrlScanned(UrlScanningContext db, ILogger<SaveResultsWhenUrlScanned> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Handle(UrlScanned @event)
        {
            _logger.LogInformation($"Saving scan results for URL with ID: {@event.UrlId}");
            
            var info = await _db.UrlInfos.FindAsync(@event.UrlId);
            info.HasGoogle = @event.HasGoogle;
            info.PhoneNumbers = @event.PhoneNumbers;
            info.ScanDuration = @event.ScanDuration;
            info.LastTimeScanned = @event.LastTimeScanned;

            await _db.SaveChangesAsync();
        }
    }
}