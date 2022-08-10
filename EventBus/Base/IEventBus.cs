namespace Project.BillingProcessing.Api.EventBus.Base
{
    public interface IEventBus
    {
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    

    }
}
