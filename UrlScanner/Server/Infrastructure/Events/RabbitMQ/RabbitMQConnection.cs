using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace UrlScanner.Server.Infrastructure.Events.RabbitMQ
{
    internal sealed class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private readonly IConnectionFactory _factory;
        private readonly ILogger<RabbitMQConnection> _logger;
        
        private readonly object _syncRoot = new();
        private bool _isDisposed;

        private IConnection _connection;
        public bool IsConnected => (_connection?.IsOpen ?? false) && !_isDisposed;
        
        public RabbitMQConnection(IConnectionFactory factory, ILogger<RabbitMQConnection> logger)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public bool TryConnect()
        {
            lock (_syncRoot)
            {
                if (IsConnected) return true;
                
                _logger.LogInformation("Trying to connect RabbitMQ client.");
                _connection = _factory.CreateConnection();
            }

            if (IsConnected)
            {
                _logger.LogInformation("RabbitMQ client connected.");
                return true;
            }
            
            _logger.LogError("RabbitMQ failed to connect.");
            return false;
        }

        public IModel CreateChannel() => _connection.CreateModel();
        
        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            
            _connection?.Dispose();
        }
    }
}