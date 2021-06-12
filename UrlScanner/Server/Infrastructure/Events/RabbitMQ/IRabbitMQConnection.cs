using RabbitMQ.Client;

namespace UrlScanner.Server.Infrastructure.Events.RabbitMQ
{
    internal interface IRabbitMQConnection
    {
        public const string ExchangeName = "EventBusExchange";
            
        public bool IsConnected { get; }

        public bool TryConnect();
        public IModel CreateChannel();
    }
}