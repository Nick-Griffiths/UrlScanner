using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UrlScanner.Server.Application.Pipelining.Pipelines.Decorators
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class TimedPipeline : IPipeline
    {
        private readonly IPipeline _pipeline;
        private readonly ILogger<TimedPipeline> _logger;

        public TimedPipeline(IPipeline pipeline, ILogger<TimedPipeline> logger)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(CancellationToken stopToken)
        {
            var watch = Stopwatch.StartNew();

            await _pipeline.Execute(stopToken);

            _logger.LogInformation($"Total Pipeline Execution Time: {watch.Elapsed}.");
        }
    }
}