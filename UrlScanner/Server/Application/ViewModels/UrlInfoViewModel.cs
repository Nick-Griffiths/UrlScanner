using System.Diagnostics.CodeAnalysis;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Extensions;

namespace UrlScanner.Server.Application.ViewModels
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal sealed class UrlInfoViewModel
    {
        public const string DateTimeFormat = "dd/MM/yy hh:mm:ss:fff";
        
        public string Id { get; init; }
        public string Name { get; init; }
        public string Url { get; init; }
        public string HasGoogle { get; init; }
        public string PhoneNumbers { get; init; }
        public string ScanDurationInMilliseconds { get; init; }
        public string LastTimeScanned { get; init; }
    }
    
    internal static class UrlScannerResultExtensions
    {
        internal static UrlInfoViewModel ToViewModel(this UrlInfo urlInfo)
        {
            var model = new UrlInfoViewModel
            {
                Id = urlInfo.Id.ToString(),
                Name = urlInfo.Name,
                Url = urlInfo.Url,
                HasGoogle = urlInfo.HasGoogle ? "Yes" : "No",
                PhoneNumbers = urlInfo.PhoneNumbers.IsNullOrWhiteSpace() ? "N/A" : urlInfo.PhoneNumbers,
                ScanDurationInMilliseconds = urlInfo.ScanDuration?.TotalMilliseconds.ToString("F0") ?? "N/A",
                LastTimeScanned = urlInfo.LastTimeScanned?.ToString(UrlInfoViewModel.DateTimeFormat) ?? "N/A"
            };
    
            return model;
        }
    }
}