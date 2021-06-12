using System;
using System.Diagnostics.CodeAnalysis;

namespace UrlScanner.Server.Infrastructure.Events
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class Event
    {
        internal Guid Id { get; }
        internal DateTime CreatedAt { get; }

        internal Event()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
}