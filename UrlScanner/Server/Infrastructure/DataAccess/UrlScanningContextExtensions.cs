using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UrlScanner.Server.Infrastructure.DataAccess
{
    internal static class UrlScanningContextExtensions
    {
        internal static async Task InitialiseDatabase(this UrlScanningContext context)
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
        }
    }
}