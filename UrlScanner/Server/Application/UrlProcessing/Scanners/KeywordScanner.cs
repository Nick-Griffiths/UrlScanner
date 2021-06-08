using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlProcessing.Scanners
{
    internal sealed class KeywordScanner : IUrlScanner
    {
        private readonly IOptionsMonitor<ScannerOptions> _options;
        private readonly ILogger<KeywordScanner> _logger;

        internal KeywordScanner(IOptionsMonitor<ScannerOptions> options, ILogger<KeywordScanner> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Scan(string htmlContent, UrlInfo urlInfo)
        {
            var searchString = _options.CurrentValue.SearchString;
            
            _logger.LogInformation($"Performing keyword scan on {urlInfo.Url} looking for {searchString}.");
            
            urlInfo.HasGoogle = htmlContent.Contains(searchString);
        }
    }
}