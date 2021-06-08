using System;
using static System.AttributeTargets;

namespace UrlScanner.Server.Infrastructure
{
    [AttributeUsage(Property)]
    internal sealed class CsvIgnoreAttribute : Attribute { }
}