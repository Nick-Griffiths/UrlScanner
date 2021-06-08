using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using UrlScanner.Server.Application.Pipelining.Pipelines;
using static System.Threading.Tasks.Task;

namespace UrlScanner.Server.Application.ScannerService
{
    internal sealed class ScannerService : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;
        private readonly IOptionsMonitor<ScannerServiceOptions> _options;

        public ScannerService(IServiceScopeFactory factory, IOptionsMonitor<ScannerServiceOptions> options)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while(!stopToken.IsCancellationRequested)
            {
                if (_options.CurrentValue.ExecutionIsEnabled)
                {
                    using var scope = _factory.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<IPipeline>().Execute(stopToken);
                }

                await Delay(_options.CurrentValue.ExecutionIntervalInMilliseconds, stopToken);
            }        
        }
    }
}