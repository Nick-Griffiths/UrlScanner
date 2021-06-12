using System;
using System.Collections.Generic;

namespace UrlScanner.Server.Infrastructure.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var element in source) action(element, index++);
        }
    }
}