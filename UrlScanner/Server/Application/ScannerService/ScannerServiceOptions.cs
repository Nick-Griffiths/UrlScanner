using System.Diagnostics.CodeAnalysis;

namespace UrlScanner.Server.Application.ScannerService
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal sealed class ScannerServiceOptions
    {
        public bool ExecutionIsEnabled { get; set; }
        public int ExecutionIntervalInMilliseconds { get; set; }
        public bool UseEventBus { get; set; }
    }
}