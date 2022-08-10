var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRabbitMQConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["HostName"],
        DispatchConsumersAsync = true,
        Uri = new Uri(builder.Configuration["EventBusConnection"])
    };

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusUserName"]))
    {
        factory.UserName = builder.Configuration["EventBusUserName"];
    }

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusPassword"]))
    {
        factory.Password = builder.Configuration["EventBusPassword"];
    }

    var retryCount = 5;
    if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
    }

    return new DefaultRabbitMQConnection(factory, logger, retryCount);
});


builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
{
    var queueName = builder.Configuration["QueueRabbit"];
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQConnection>();
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    var serviceprovide = sp.GetRequiredService<IServiceProvider>();

    var retryCount = 5;
    if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
    }

    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, queueName, serviceprovide, retryCount);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
