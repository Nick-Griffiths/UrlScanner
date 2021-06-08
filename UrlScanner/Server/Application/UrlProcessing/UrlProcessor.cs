using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlScanner.Server.Application.UrlLoading;
using UrlScanner.Server.Application.UrlProcessing.Scanners;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlProcessing
{
    internal sealed class UrlProcessor : IUrlProcessor
    {
        private readonly IUrlLoader _loader;
        private readonly IEnumerable<IUrlScanner> _scanners;
        private readonly ILogger<UrlProcessor> _logger;
        private readonly ScannerOptions _options;
        
        internal UrlProcessor(
            IUrlLoader loader,
            IEnumerable<IUrlScanner> scanners,
            ILogger<UrlProcessor> logger,
            IOptionsSnapshot<ScannerOptions> options)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _scanners = scanners ?? throw new ArgumentNullException(nameof(scanners));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
        }

        public async Task<UrlInfo> Process(UrlInfo urlInfo, CancellationToken stopToken)
        {
            _logger.LogInformation($"Scanning {urlInfo.Url} with scanners: {_options.EnabledScanners}.");

            var htmlContent = await _loader.Load(urlInfo, stopToken);
            Parallel.ForEach(_scanners.Where(ScannerIsEnabled), s => s.Scan(htmlContent, urlInfo));
            
            return urlInfo;
        }

        private bool ScannerIsEnabled(IUrlScanner scanner)
        {
            return _options.EnabledScanners.Any(s => scanner.GetType().Name.StartsWith(s));
        }
    }
}