using System.Diagnostics.CodeAnalysis;

namespace UrlScanner.Server.Application.Pipelining.Pipelines
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal sealed class PipelineOptions
    {
        public PipelineType Type { get; set; }
        public int MaxConcurrency { get; set; }
        public int Capacity { get; set; }
    }
}