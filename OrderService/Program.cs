using Domain.Interfaces;
using Infrasctructure.Context;
using Infrasctructure.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//Add Db
builder.Services.AddDbContext<TesteAmbevContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Singleton);

//Inject App Dependencies
builder.Services.AddSingleton<IOrderService, Application.Services.OrderService>();

//Inject App Repositories/Data Dependencies
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

var host = builder.Build();
host.Run();
