using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Extensions;

namespace UrlScanner.Server.Application.UrlProcessing.Scanners
{
    internal sealed class PhoneNumberScanner : IUrlScanner
    {
        private readonly IOptionsMonitor<ScannerOptions> _options;
        private readonly ILogger<PhoneNumberScanner> _logger;

        internal PhoneNumberScanner(IOptionsMonitor<ScannerOptions> options, ILogger<PhoneNumberScanner> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Scan(string htmlContent, UrlInfo urlInfo)
        {
            var util = PhoneNumberUtil.GetInstance();
            var regionCode = _options.CurrentValue.PhoneNumberRegionCode;
            
            _logger.LogInformation($"Performing phone number scan of {urlInfo.Url} using region code {regionCode}.");

            var matches = util.FindNumbers(htmlContent, regionCode);
            var numbers = matches.Select(n => n.RawString).Distinct().ToList();
            var numbersString = string.Join(", ", numbers);
            var logString = numbersString.IsNullOrWhiteSpace() ? "no numbers" : numbersString;
            
            _logger.LogInformation($"Phone number scan of {urlInfo.Url} found {logString}.");

            urlInfo.PhoneNumbers = numbersString;
        }
    }
}