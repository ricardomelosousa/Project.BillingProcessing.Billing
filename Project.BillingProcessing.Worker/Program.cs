using GrpcChargeApi;
using GrpcCustomers;
using Project.BillingProcessing.Charge.Api.Service;
using Project.BillingProcessing.Worker;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton<BillingGrpcService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddGrpcClient<CustomerProtoService.CustomerProtoServiceClient>
        (options=> options.Address = new Uri(configuration["GrpcUri:CustomerUrl"]));
        services.AddGrpcClient<ChargeProtoService.ChargeProtoServiceClient>
       (options => options.Address = new Uri(configuration["GrpcUri:ChargeUrl"]));  
        services.AddHostedService<Worker>();       
    })
    .Build();

await host.RunAsync();
