using System;
using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Options;
using UrlScanner.Server.Application.Pipelining.Pipelines;
using UrlScanner.Server.Application.Pipelining.Pipelines.Decorators;
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
            }).InstancePerLifetimeScope();
            builder.RegisterDecorator<TimedPipeline, IPipeline>();
        }
    }
}