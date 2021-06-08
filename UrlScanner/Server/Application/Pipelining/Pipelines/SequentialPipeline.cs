using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlScanner.Server.Application.UrlProcessing;

namespace UrlScanner.Server.Application.Pipelining.Pipelines
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class SequentialPipeline : Pipeline
    {
        internal SequentialPipeline(
            IUrlInfoProvider provider,
            IUrlProcessor processor,
            IUrlInfoConsumer consumer,
            ILogger<SequentialPipeline> logger) : 
            base(provider, processor, consumer, logger) { }

        public override async Task Execute(CancellationToken stopToken)
        {
            Logger.LogInformation("Executing sequential pipeline.");
            
            foreach (var urlInfo in Provider.GetUrlInfos())
            {
                var result = await Processor.Process(urlInfo, stopToken);
                await Consumer.Consume(result, stopToken);
            }
            
            Logger.LogInformation("Sequential pipeline execution completed.");

            await Consumer.CompleteConsuming(stopToken);
        }
    }
}