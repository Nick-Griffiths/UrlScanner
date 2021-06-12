namespace UrlScanner.Server.Infrastructure.Events.RabbitMQ
{
    internal interface IRabbitMQConsumer
    {
        public void Subscribe<TEvent, THandler>() where TEvent : Event where THandler : IEventHandler<TEvent>;
    }
}