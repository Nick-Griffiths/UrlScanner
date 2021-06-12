using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Text.Encoding;
using static Newtonsoft.Json.JsonConvert;
using static RabbitMQ.Client.ExchangeType;
using static UrlScanner.Server.Infrastructure.Events.RabbitMQ.IRabbitMQConnection;

namespace UrlScanner.Server.Infrastructure.Events.RabbitMQ
{
    internal sealed class RabbitMQConsumer : IRabbitMQConsumer, IDisposable
    {
        private const string QueueName = "EventBusQueue";

        private readonly IRabbitMQConnection _connection;
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<RabbitMQConsumer> _logger;

        private readonly Dictionary<string, HashSet<Type>> _handlerTypes = new();
        private readonly HashSet<Type> _eventTypes = new();

        private IModel _channel;

        private bool _isDisposed;

        public RabbitMQConsumer(
            IRabbitMQConnection connection,
            IServiceScopeFactory factory,
            ILogger<RabbitMQConsumer> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Subscribe<TEvent, THandler>() where TEvent : Event where THandler : IEventHandler<TEvent>
        {
            if (!(_connection.IsConnected || _connection.TryConnect())) return;

            if (_channel == null)
            {
                InitialiseChannel();
                StartConsumption();
            }

            _logger.LogInformation($"Subscribing handler {typeof(THandler).Name} " +
                                   $"to event {typeof(TEvent).Name}.");

            using var channel = _connection.CreateChannel();
            var eventName = typeof(TEvent).Name;          
            channel.QueueBind(QueueName, ExchangeName, eventName);
            
            if (!_handlerTypes.ContainsKey(eventName)) _handlerTypes.Add(eventName, new HashSet<Type>());
            _handlerTypes[eventName].Add(typeof(THandler));
            _eventTypes.Add(typeof(TEvent));
        }

        private void InitialiseChannel()
        {
            _logger.LogInformation("Initialising RabbitMQ consumer channel.");

            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(ExchangeName, Direct);
            _channel.QueueDeclare(QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void StartConsumption()
        {
            _logger.LogInformation("Starting consumption of RabbitMQ events.");
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;
            _channel.BasicConsume(QueueName, autoAck: false, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var eventString = UTF8.GetString(eventArgs.Body.Span);
            
            _logger.LogInformation($"Received RabbitMQ event: {eventString}.");
            
            await ProcessEvent(eventString, eventName);
            _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventString, string eventName)
        {
            _logger.LogInformation($"Processing RabbitMQ event {eventName}.");

            if (!_handlerTypes.TryGetValue(eventName, out var handlerTypes))
            {
                _logger.LogWarning($"No subscriptions for event {eventName}.");
                return;
            }

            foreach (var type in handlerTypes) await Handle(eventName, eventString, type);
        }

        private async Task Handle(string eventName, string eventString, Type handlerType)
        {
            using var scope = _factory.CreateScope();
            var handler = scope.ServiceProvider.GetService(handlerType);
            if (handler == null)
            {
                _logger.LogWarning($"No handler registered for {handlerType.Name}.");
                return;
            }

            var eventType = _eventTypes.Single(t => t.Name == eventName);
            var @event = DeserializeObject(eventString, eventType);
            var genericHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            await (Task) genericHandlerType.GetMethod("Handle").Invoke(handler, new[] {@event});
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _channel?.Dispose();
        }
    }
}