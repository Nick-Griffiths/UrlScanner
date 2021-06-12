using System.Threading.Tasks;

namespace UrlScanner.Server.Infrastructure.Events
{
    internal interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
    
    internal interface IEventHandler { }
}