using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UrlScanner.Server.Application.Events;
using UrlScanner.Server.Infrastructure.Events;

namespace UrlScanner.Server.Infrastructure.Startup
{
    internal static class ApplicationBuilderExtensions
    {
        internal static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IEventBus>();
            bus.Subscribe<UrlScanned, SaveResultsWhenUrlScanned>();
            bus.Subscribe<UrlScanned, EmailInfoWhenUrlScanned>();
        }
    }
}