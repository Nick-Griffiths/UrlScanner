namespace UrlScanner.Server.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        internal static bool IsNullOrWhiteSpace(this string theString) => string.IsNullOrWhiteSpace(theString);
    }
}