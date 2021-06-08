using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrlScanner.Server.Infrastructure.DataAccess
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class UrlInfo
    {
        [CsvIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [CsvIgnore]
        public bool HasGoogle { get; set; }
        [CsvIgnore]
        public string PhoneNumbers { get; set; }
        [CsvIgnore]
        public TimeSpan? ScanDuration { get; set; }
        [CsvIgnore]
        public DateTime? LastTimeScanned { get; set; }
    }
}