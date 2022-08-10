namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.ServiceBus
{
    public class DefaultServiceBusConnection : IServiceBusConnection
    {
        private readonly string _serviceBusConnectionString;
        private ServiceBusClient _queueClient;
        private ServiceBusAdministrationClient _subscriptionClient;

        bool _disposed;

        public DefaultServiceBusConnection(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _subscriptionClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);
            _queueClient = new ServiceBusClient(_serviceBusConnectionString);
            
        }   

        public ServiceBusClient QueueClient
        {
            get
            {
                if (_queueClient.IsClosed)
                {
                    _queueClient = new ServiceBusClient(_serviceBusConnectionString);
                }
                return _queueClient;
            }
        }

        public ServiceBusAdministrationClient AdministrationClient
        {
            get
            {
                return _subscriptionClient;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _queueClient.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
