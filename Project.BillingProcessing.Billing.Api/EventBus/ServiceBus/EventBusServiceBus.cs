namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.ServiceBus
{
    public class EventBusServiceBus : IEventBus, IDisposable
    {
        private readonly IServiceBusConnection _serviceBusConnection;
        private readonly ILogger<EventBusServiceBus> _logger;
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private ServiceBusSender _sender;
        private ServiceBusProcessor _processor;
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private readonly string _topicName = "";
        private readonly string _subscriptionName;
        private readonly IServiceProvider _serviceProvider;

        public EventBusServiceBus(IServiceBusConnection serviceBusConnection,
     ILogger<EventBusServiceBus> logger, IEventBusSubscriptionsManager eventBusSubscriptionsManager, string subscriptionClientName, IServiceProvider serviceProvider)
        {
            _serviceBusConnection = serviceBusConnection;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionName = subscriptionClientName;
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager;
            _sender = _serviceBusConnection.QueueClient.CreateSender(_subscriptionName);
            ServiceBusProcessorOptions options = new ServiceBusProcessorOptions { MaxConcurrentCalls = 2, AutoCompleteMessages = false };
            _processor = _serviceBusConnection.QueueClient.CreateProcessor(_subscriptionName, options);
            _serviceProvider = serviceProvider;
            RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
        }
        public void Publish(IntegrationEvent @event)
        {
            if (@event == null)
            {
                _logger.LogError("A mensagem não pode ser nula para publicação no servicebus");
                throw new Exception("A mensagem não pode ser nula para publicação no servicebus");
            }
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            var message = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = new BinaryData(body),
                Subject = eventName,
            };
            _sender.SendMessageAsync(message)
           .GetAwaiter()
           .GetResult();
        }
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var containsKey = _eventBusSubscriptionsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    //Implentar para consumo topic
                    //_serviceBusConnection.AdministrationClient.CreateRuleAsync(_topicName, _subscriptionName, new CreateRuleOptions
                    //{
                    //    Filter = new CorrelationRuleFilter() { Subject = eventName },
                    //    Name = eventName
                    //}).GetAwaiter().GetResult();
                    _serviceBusConnection.AdministrationClient.CreateQueueAsync(eventName).GetAwaiter().GetResult();
                }
                catch (ServiceBusException)
                {
                    _logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
                }
            }
            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);
            _eventBusSubscriptionsManager.AddSubscription<T, TH>();
        }
        public void Dispose()
        {
            _processor.CloseAsync().GetAwaiter().GetResult();
        }
        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);
            _eventBusSubscriptionsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);
            _eventBusSubscriptionsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            try
            {
            }
            catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
            {
                _logger.LogWarning("A mensagem da entidade {eventName} não pode ser localizada.", eventName);
            }
            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);
        }

        private async Task RegisterSubscriptionClientMessageHandlerAsync()
        {
            _processor.ProcessMessageAsync +=
                async (args) =>
                {
                    var eventName = $"{args.Message.Subject}{INTEGRATION_EVENT_SUFFIX}";
                    string messageData = args.Message.Body.ToString();
                    if (await ProcessEvent(eventName, messageData))
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                };

            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            var ex = args.Exception;
            var context = args.ErrorSource;
            _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);
            return Task.CompletedTask;
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            var processed = false;
            if (_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    dynamic handlerInstance = _serviceProvider.GetService(subscription.HandlerType);
                    var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handlerInstance, new object[] { integrationEvent });
                }
                processed = true;
            }
            return processed;
        }
    }
}
