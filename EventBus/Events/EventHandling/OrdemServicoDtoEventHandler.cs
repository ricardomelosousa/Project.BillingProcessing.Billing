using Project.BillingProcessing.Api.EventBus.Base;


namespace Project.BillingProcessing.Billing.Api.EventBus.Events.EventHandling
{
    public class ChargeEventHandler : IIntegrationEventHandler<ChargeIntegrationEvent>
    {
        private readonly ILogger<ChargeEventHandler> _logger;

        private readonly IServiceProvider _serviceProvider;
        public ChargeEventHandler(ILogger<ChargeEventHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Handle(ChargeIntegrationEvent intEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ordemServicoAppService = scope.ServiceProvider.GetRequiredService<IOrdemServicoAppService>();
                 _ordemServicoAppService.SalvarOrdemServico(intEvent);
            }       
            _logger.LogInformation("----- Handling integration event: {OrdemServicoId} - ({@IntegrationEvent})", intEvent.OrdemServicoId, intEvent);
            await Task.CompletedTask;
        }
       
    }
}
