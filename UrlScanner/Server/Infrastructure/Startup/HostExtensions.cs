using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Infrastructure.Startup
{
    internal static class HostExtensions
    {
        public static async Task<IHost> InitialiseDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<UrlScanningContext>();

            await context.InitialiseDatabase();

            return host;
        }
    }
}