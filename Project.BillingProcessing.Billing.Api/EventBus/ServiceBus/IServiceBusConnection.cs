namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.ServiceBus
{
    public interface IServiceBusConnection : IDisposable
    {
        ServiceBusClient QueueClient { get; }
        ServiceBusAdministrationClient AdministrationClient { get; }

    }
}
