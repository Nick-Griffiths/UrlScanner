using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace UrlScanner.Server.Infrastructure.Startup
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = await CreateHostBuilder(args).Build().InitialiseDatabase();
            
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment()) builder.AddUserSecrets<Program>();
                });
        }
    }
}