using Infrasctructure.Context;
using Microsoft.EntityFrameworkCore;
using OrderService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//Add Db
builder.Services.AddDbContext<TesteAmbevContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
});

var host = builder.Build();
host.Run();
