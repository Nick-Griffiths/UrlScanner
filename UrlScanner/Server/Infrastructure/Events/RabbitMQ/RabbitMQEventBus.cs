using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using static System.Text.Encoding;
using static Newtonsoft.Json.JsonConvert;
using static RabbitMQ.Client.ExchangeType;
using static UrlScanner.Server.Infrastructure.Events.RabbitMQ.IRabbitMQConnection;

namespace UrlScanner.Server.Infrastructure.Events.RabbitMQ
{
    internal sealed class RabbitMQEventBus : IEventBus
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IRabbitMQConsumer _consumer;
        private readonly ILogger<RabbitMQEventBus> _logger;
        
        public RabbitMQEventBus(
            IRabbitMQConnection connection,
            IRabbitMQConsumer consumer,
            ILogger<RabbitMQEventBus> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public void Publish(Event @event)
        {
            if (!(_connection.IsConnected || _connection.TryConnect())) return;

            using var channel = _connection.CreateChannel();
            channel.ExchangeDeclare(ExchangeName, Direct);

            var json = SerializeObject(@event);
            var body = UTF8.GetBytes(json);
            
            _logger.LogInformation($"Publishing event with ID {@event.Id} to RabbitMQ.");
            channel.BasicPublish(ExchangeName, @event.GetType().Name, basicProperties: null, body);
        }
        
        public void Subscribe<TEvent, THandler>() where TEvent : Event where THandler : IEventHandler<TEvent>
        {
            _consumer.Subscribe<TEvent, THandler>();
        }
    }
}