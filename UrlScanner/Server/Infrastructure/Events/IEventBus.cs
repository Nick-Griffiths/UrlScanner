namespace UrlScanner.Server.Infrastructure.Events
{
    internal interface IEventBus
    {
        public void Publish(Event @event);
        public void Subscribe<TEvent, THandler>() where TEvent : Event where THandler : IEventHandler<TEvent>;
    }
}