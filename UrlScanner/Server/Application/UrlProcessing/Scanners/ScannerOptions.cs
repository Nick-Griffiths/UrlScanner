using System.Diagnostics.CodeAnalysis;

namespace UrlScanner.Server.Application.UrlProcessing.Scanners
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal sealed class ScannerOptions
    {
        public string EnabledScanners { get; set; }
        public string SearchString { get; set; }
        public string PhoneNumberRegionCode { get; set; }
    }
}