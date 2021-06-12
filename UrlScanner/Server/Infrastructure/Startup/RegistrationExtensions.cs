using System;
using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UrlScanner.Server.Application.Pipelining;
using UrlScanner.Server.Application.Pipelining.Pipelines;
using UrlScanner.Server.Application.Pipelining.Pipelines.Decorators;
using UrlScanner.Server.Application.ScannerService;
using UrlScanner.Server.Infrastructure.Events;
using static System.Reflection.Assembly;
using static System.Reflection.BindingFlags;
using static UrlScanner.Server.Application.Pipelining.Pipelines.PipelineType;

namespace UrlScanner.Server.Infrastructure.Startup
{
    internal static class RegistrationExtensions
    {
        internal static void UsingAnyConstructor<TLimit, TConcreteActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteActivatorData, TRegistrationStyle> builder)
            where TConcreteActivatorData : ReflectionActivatorData
        {
            builder.FindConstructorsWith(t => t.GetConstructors(Public | NonPublic | Instance));
        }

        internal static void RegisterPipeline(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetExecutingAssembly())
                .Where(t => typeof(IPipeline).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .UsingAnyConstructor();
            
            builder.Register<IPipeline>(c =>
            {
                var pipelineType = c.Resolve<IOptionsMonitor<PipelineOptions>>().CurrentValue.Type;
                return pipelineType switch
                {
                    Sequential => c.Resolve<SequentialPipeline>(),
                    Parallel => c.Resolve<ParallelPipeline>(),

                    _ => throw new ArgumentOutOfRangeException($"Invalid PipelineType: {pipelineType}.")
                };
            });
            builder.RegisterDecorator<TimedPipeline, IPipeline>();
        }

        internal static void RegisterPipelineProvidersAndConsumers(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetExecutingAssembly())
                .Where(t => typeof(IUrlInfoProvider).IsAssignableFrom(t) ||
                            typeof(IUrlInfoConsumer).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .UsingAnyConstructor();
            
            builder.Register<IUrlInfoProvider>(c =>
            {
                var useEventBus = c.Resolve<IOptionsMonitor<ScannerServiceOptions>>().CurrentValue.UseEventBus;
                return useEventBus ?
                    c.Resolve<EfCoreUrlInfoProvider>() :
                    c.Resolve<EfCoreUrlInfoProviderConsumer>();
            });
            
            builder.Register<IUrlInfoConsumer>(c =>
            {
                var useEventBus = c.Resolve<IOptionsMonitor<ScannerServiceOptions>>().CurrentValue.UseEventBus;
                return useEventBus ?
                    c.Resolve<EventBusUrlInfoConsumer>() :
                    c.Resolve<EfCoreUrlInfoProviderConsumer>();
            });
        }

        internal static void RegisterEventBusAndHandlers(this ContainerBuilder builder, IConfiguration config)
        {
            builder.Register<IConnectionFactory>(_ => new ConnectionFactory
            {
                HostName = config["EventBusOptions:HostName"],
                UserName = config["EventBusOptions:UserName"],
                Password = config["EventBusOptions:Password"],
                DispatchConsumersAsync = true
                
            }).SingleInstance();
            
            builder.RegisterAssemblyTypes(GetExecutingAssembly())
                .Where(t => t.Name.StartsWith("RabbitMQ"))
                .SingleInstance()
                .AsImplementedInterfaces()
                .UsingAnyConstructor();
            
            builder.RegisterAssemblyTypes(GetExecutingAssembly())
                .Where(t => typeof(IEventHandler).IsAssignableFrom(t))
                .UsingAnyConstructor();
        }
    }
}