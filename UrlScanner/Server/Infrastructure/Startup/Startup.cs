using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using UrlScanner.Server.Application.Pipelining.Pipelines;
using UrlScanner.Server.Application.ScannerService;
using UrlScanner.Server.Application.UrlLoading;
using UrlScanner.Server.Application.UrlProcessing;
using UrlScanner.Server.Application.UrlProcessing.Decorators;
using UrlScanner.Server.Application.UrlProcessing.Scanners;
using UrlScanner.Server.Infrastructure.CsvParsing;
using UrlScanner.Server.Infrastructure.DataAccess;
using UrlScanner.Server.Infrastructure.Email;
using static System.Reflection.Assembly;

namespace UrlScanner.Server.Infrastructure.Startup
{
    internal sealed class Startup
    {
        private IConfiguration Config { get; }
        
        public Startup(IConfiguration config) => Config = config;

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new SqlConnectionStringBuilder(Config.GetConnectionString("DefaultConnection"))
            {
                Password = Config["DbPassword"]
            };
            services.AddDbContext<UrlScanningContext>(o => o.UseSqlServer(builder.ConnectionString));  
            
            services.Configure<ScannerServiceOptions>(Config.GetSection(nameof(ScannerServiceOptions)));
            services.Configure<PipelineOptions>(Config.GetSection(nameof(PipelineOptions)));
            services.Configure<ScannerOptions>(Config.GetSection(nameof(ScannerOptions)));
            services.Configure<EmailOptions>(Config.GetSection(nameof(EmailOptions)));

            services.AddControllers(o =>
            {
                o.InputFormatters.Add(new CsvInputFormatter());
                o.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            });
            
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "Client/build");
            services.AddHostedService<ScannerService>();
            services.AddHttpClient<IUrlLoader, UrlLoader>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterPipeline();
            builder.RegisterPipelineProvidersAndConsumers();

            builder.RegisterEventBusAndHandlers(Config);

            builder.RegisterType<UrlProcessor>()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .UsingAnyConstructor();
            builder.RegisterDecorator<TimedUrlProcessor, IUrlProcessor>();
            
            builder.RegisterAssemblyTypes(GetExecutingAssembly())
                .Where(t => typeof(IUrlScanner).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces()
                .UsingAnyConstructor();
            
            builder.RegisterType<EmailService>()
                .SingleInstance()
                .AsImplementedInterfaces()
                .UsingAnyConstructor();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Client";

                if (env.IsDevelopment()) spa.UseReactDevelopmentServer(npmScript: "start");
            });

            app.ConfigureEventBus();
        }
    }
}