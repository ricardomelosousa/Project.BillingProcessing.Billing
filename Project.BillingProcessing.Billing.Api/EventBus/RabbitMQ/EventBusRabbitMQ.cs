namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string QueueSuffix = ".integrador.os";
        const string RabbitIntegradorOSSuffix = ".exchange.fanout";
        const string RabbitExchange = ".exchange.fanout";
        const string RabbitPubExchange = ".allStore.exchange.fanout";
        private const string IntegradorEventSuffix = "IntegrationEvent";
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly int _retryCount;
        private readonly IServiceProvider _serviceProvider;
        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(IRabbitMQConnection rabbitMQConnection, ILogger<EventBusRabbitMQ> logger, IEventBusSubscriptionsManager eventBusSubscriptionsManager,
            string queueName, IServiceProvider serviceProvider, int retryCount = 5)
        {
            _rabbitMQConnection = rabbitMQConnection ?? throw new ArgumentNullException(nameof(rabbitMQConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            _serviceProvider = serviceProvider;
            _consumerChannel = CreateConsumerChannel();
            _retryCount = retryCount;
            _eventBusSubscriptionsManager.OnEventRemoved += _eventBusSubscriptionsManager_OnEventRemoved;

        }



        public void Publish(IntegrationEvent @event)
        {
            if (!_rabbitMQConnection.IsConnected)
            {
                _rabbitMQConnection.TryConnect();
            }
            if (@event == null)
            {
                _logger.LogError("A mensagem não pode ser nula para publicação no rabbit");
                throw new Exception("A mensagem não pode ser nula para publicação no rabbit");
            }


            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Não conseguiu publicar mensagem de  event: {EventId} depois de {Timeout}s ({ExceptionMessage})", @event.IntegrationId, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name.Replace(IntegradorEventSuffix, "").ToLower();

            //_logger.LogTrace("Criando conexão RabbitMQ to publish event: {EventId} ({EventName})", @event.IntegrationId, eventName);

            using (var channel = _rabbitMQConnection.CreateModel())
            {
                var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;                    
                    channel.BasicPublish(
                        exchange: $"{eventName}{RabbitPubExchange }",
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _eventBusSubscriptionsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
            _logger.LogInformation("Subscribing to event {EventName}", eventName);
            _eventBusSubscriptionsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }
        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                eventName = eventName.Replace(IntegradorEventSuffix, "").ToLower();
                if (!_rabbitMQConnection.IsConnected)
                {
                    _rabbitMQConnection.TryConnect();
                }
                _consumerChannel.QueueBind(queue: _queueName,
                                    exchange: $"{eventName}{RabbitExchange}",
                                    routingKey: eventName);
            }
        }
        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
            _eventBusSubscriptionsManager.Clear();
        }
        private void _eventBusSubscriptionsManager_OnEventRemoved(object? sender, string e)
        {
            throw new NotImplementedException();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_rabbitMQConnection.IsConnected)
            {
                _rabbitMQConnection.TryConnect();
            }

            _logger.LogTrace("Criando canal de consumo RabbitMQ");

            var channel = _rabbitMQConnection.CreateModel();

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recriando canal de consumo RabbitMQ");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Iniciando consumo de fila no RabbitMQ");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {

                await ProcessEvent($"{eventName.Replace(QueueSuffix, "")}{ IntegradorEventSuffix.ToLower()}", message);
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                //_consumerChannel.BasicReject(eventArgs.DeliveryTag, true);
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, true, true);
                _logger.LogWarning(ex, "----- ERROR  \"{Message}\"", message);
            }


        }

        private async Task ProcessEvent(string eventName, string message)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }

            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
            {

                var subscriptions = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    dynamic handlerInstance = _serviceProvider.GetService(subscription.HandlerType) ?? throw new ArgumentNullException(nameof(_serviceProvider));
                    var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle").Invoke(handlerInstance, new object[] { integrationEvent });
                }
            }
            else
            {
                _logger.LogWarning("Não localizado assinatura RabbitMQ para evento: {EventName}", eventName);
                throw new Exception("Não localizado assinatura RabbitMQ");
            }
        }
    }
}
