using Application.Services;
using Domain.Interfaces;
using Infrasctructure.Context;
using Infrasctructure.Queues;
using Infrasctructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderService;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

//Configure RabbitMQ
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton(serviceProvider =>
{
    var rabbitConfig = serviceProvider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

    var factory = new ConnectionFactory()
    {
        HostName = rabbitConfig.HostName,
        Port = rabbitConfig.Port,
        UserName = rabbitConfig.UserName,
        Password = rabbitConfig.Password
    };

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    //Queue configuration
    QueueConfiguration.ConfigureQueues(channel);

    return channel;
});

builder.Services.AddHostedService<Worker>();

//Add Db
builder.Services.AddDbContext<TesteAmbevContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
    options.LogTo(Console.WriteLine, LogLevel.None);
}, ServiceLifetime.Singleton);

//Inject App Dependencies
builder.Services.AddSingleton<IOrderService, Application.Services.OrderService>();
builder.Services.AddSingleton<IOrderNotificationService, OrderNotificationService>();

//Inject App Repositories/Data Dependencies
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

var host = builder.Build();
host.Run();