using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Open.ChannelExtensions;
using UrlScanner.Server.Application.UrlProcessing;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Pipelining.Pipelines
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class ParallelPipeline : Pipeline
    {
        private readonly IOptionsMonitor<PipelineOptions> _options;
        
        internal ParallelPipeline(
            IUrlInfoProvider provider,
            IUrlProcessor processor,
            IUrlInfoConsumer consumer,
            IOptionsMonitor<PipelineOptions> options,
            ILogger<ParallelPipeline> logger) : 
            base(provider, processor, consumer, logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task Execute(CancellationToken stopToken)
        {
            StopToken = stopToken;
            
            var capacity = _options.CurrentValue.Capacity;
            var maxConcurrency = _options.CurrentValue.MaxConcurrency;
            
            Logger.LogInformation("Executing parallel pipeline.  " +
                                   $"Capacity: {capacity}.  Max concurrency: {maxConcurrency}.");

            await Provider.GetUrlInfos()
                .ToChannel(cancellationToken: stopToken)
                .PipeAsync(maxConcurrency, capacity: capacity, transform: Process, cancellationToken: stopToken)
                .ReadAllAsync(Consume, stopToken);
            
            Logger.LogInformation("Parallel pipeline execution completed.");

            await Consumer.CompleteConsuming(stopToken);
        }
        
        private async ValueTask<UrlInfo> Process(UrlInfo urlInfo) => await Processor.Process(urlInfo, StopToken);
        private async ValueTask Consume(UrlInfo result) => await Consumer.Consume(result, StopToken);
    }
}