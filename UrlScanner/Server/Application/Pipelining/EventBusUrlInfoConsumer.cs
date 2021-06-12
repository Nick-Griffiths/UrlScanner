using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlScanner.Server.Application.Events;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Events;
using static System.Threading.Tasks.Task;

namespace UrlScanner.Server.Application.Pipelining
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class EventBusUrlInfoConsumer : IUrlInfoConsumer
    {
        private readonly IEventBus _bus;
        private readonly ILogger<EventBusUrlInfoConsumer> _logger;

        internal EventBusUrlInfoConsumer(IEventBus bus, ILogger<EventBusUrlInfoConsumer> logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Consume(UrlInfo urlInfo, CancellationToken stopToken)
        {
            var @event = new UrlScanned(
                urlInfo.Id,
                urlInfo.HasGoogle,
                urlInfo.PhoneNumbers,
                urlInfo.ScanDuration.GetValueOrDefault(),
                urlInfo.LastTimeScanned.GetValueOrDefault()
            );
            
            _logger.LogInformation($"Publishing UrlScanned event for UrlInfo with ID: {urlInfo.Id}");
            _bus.Publish(@event);
            
            return CompletedTask;
        }

        public Task CompleteConsuming(CancellationToken stopToken) => CompletedTask;
    }
}