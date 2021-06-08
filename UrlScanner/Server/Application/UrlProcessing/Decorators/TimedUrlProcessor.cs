using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;
using static System.DateTime;

namespace UrlScanner.Server.Application.UrlProcessing.Decorators
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class TimedUrlProcessor : IUrlProcessor
    {
        private readonly IUrlProcessor _processor;

        public TimedUrlProcessor(IUrlProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public async Task<UrlInfo> Process(UrlInfo urlInfo, CancellationToken stopToken)
        {
            var watch = Stopwatch.StartNew();

            urlInfo = await _processor.Process(urlInfo, stopToken);
            
            urlInfo.ScanDuration = watch.Elapsed;
            urlInfo.LastTimeScanned = Now;

            return urlInfo;
        }
    }
}