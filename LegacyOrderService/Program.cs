using LegacyOrderService.Data.Entities;
using LegacyOrderService.Features.System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;


//Prepare everything before start the application

var services = new ServiceCollection();

services.AddDbContext<OrderDbContext>(opt =>
    opt.UseSqlite(@"Data Source=Data\orders.db"));

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

PriceOfProducts = new ConcurrentDictionary<string, double>();
ServiceProvider = services.BuildServiceProvider();


await InitCacheForPriceProduct();


public partial class Program
{
    public static ConcurrentDictionary<string, double> PriceOfProducts { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }


    public static async Task InitCacheForPriceProduct()
    {
        //Init cache for price of product
        using var scope = Program.ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new RefreshCacheProductPriceCommand());
    }

}


