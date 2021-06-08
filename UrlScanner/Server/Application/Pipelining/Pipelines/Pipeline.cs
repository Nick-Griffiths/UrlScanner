using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlScanner.Server.Application.UrlProcessing;

namespace UrlScanner.Server.Application.Pipelining.Pipelines
{
    internal abstract class Pipeline : IPipeline
    {
        protected IUrlInfoProvider Provider { get; }
        protected IUrlProcessor Processor { get; }
        protected IUrlInfoConsumer Consumer { get; }
        protected ILogger<Pipeline> Logger { get; }
        
        protected CancellationToken StopToken { get; set; }
        
        protected Pipeline(
            IUrlInfoProvider provider,
            IUrlProcessor processor,
            IUrlInfoConsumer consumer,
            ILogger<Pipeline> logger)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Processor = processor ?? throw new ArgumentNullException(nameof(processor));
            Consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public abstract Task Execute(CancellationToken stopToken);
    }
}