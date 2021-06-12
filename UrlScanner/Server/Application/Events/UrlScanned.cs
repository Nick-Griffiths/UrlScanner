using System;
using Newtonsoft.Json;
using UrlScanner.Server.Infrastructure.Events;

namespace UrlScanner.Server.Application.Events
{
    [Serializable]
    internal sealed class UrlScanned : Event
    {
        public int UrlId { get;}
        public bool HasGoogle { get;}
        public string PhoneNumbers { get; }
        public TimeSpan ScanDuration { get; }
        public DateTime LastTimeScanned { get; }
        
        [JsonConstructor]
        internal UrlScanned(
            int urlId,
            bool hasGoogle,
            string phoneNumbers,
            TimeSpan scanDuration,
            DateTime lastTimeScanned)
        {
            UrlId = urlId;
            HasGoogle = hasGoogle;
            PhoneNumbers = phoneNumbers;
            ScanDuration = scanDuration;
            LastTimeScanned = lastTimeScanned;
        }
    }
}