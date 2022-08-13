using Project.BillingProcessing.Charge.Api.Service;
using Project.BillingProcessing.Worker.Model;

namespace Project.BillingProcessing.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private BillingGrpcService _billingGrpcService;
        private IConfiguration configuration;

        public Worker(ILogger<Worker> logger, BillingGrpcService billingGrpcService, IConfiguration configuration)
        {
            _logger = logger;
            _billingGrpcService = billingGrpcService;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var response = await _billingGrpcService.GetAllCustomers(configuration.GetValue<int>("CustomersTake"));
                    if (response.Count() == 0)
                        _logger.LogInformation("Não há clientes para processamento");
                    for (int i = 0; i < response.Count(); i++)
                    {
                        try
                        {
                            await _billingGrpcService.CreateCharge(ChargesBusiness(response[i]));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical("Erro ao criar a cobrança: " + response[i], ex.Message);

                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(configuration.GetValue<int>("TimerForExecutionCallCustomers")), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message, DateTime.Now);
                   
                }
            }
        }

        public ChargeWorkerModel ChargesBusiness(CustomerModel customerModel)
        {
            var chargeValue = customerModel.Identification.ToString().Remove(2, customerModel.Identification.ToString().Length - 4);
            var charge = new ChargeWorkerModel()
            {
                Identification = (Int32)customerModel.Identification,
                dueDate = DateTime.Now,
                ChargeValue = Convert.ToDecimal(chargeValue)
            };
            return charge;
        }
    }
}