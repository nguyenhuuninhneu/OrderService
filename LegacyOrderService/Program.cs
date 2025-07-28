using LegacyOrderService.Data.Entities;
using LegacyOrderService.Features;
using LegacyOrderService.Features.System;
using LegacyOrderService.Infrastructure.Queue;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;

public class Program
{
    public static ConcurrentDictionary<string, double> PriceOfProducts { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }
    public static Channel<object> OrderEventChannel { get; set; } = Channel.CreateUnbounded<object>();
    public static List<EventWorker> OrderEventWorkers { get; set; } = new();

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {

        services.AddDbContext<OrderDbContext>(opt =>
            opt.UseSqlite(@"Data Source=Data\orders.db"));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        ServiceProvider = services.BuildServiceProvider();


    })
    .Build();


        PriceOfProducts = new ConcurrentDictionary<string, double>();

        await InitCacheForPriceProduct();
        InitiateMemoryQueue();

        //Start Test here
        using var scope = ServiceProvider.CreateScope();
        var _mediatR = scope.ServiceProvider.GetRequiredService<IMediator>();

        await _mediatR.Send(new TestOrderCommand());

        builder.Run();


    }

    public static async Task InitCacheForPriceProduct()
    {
        //Init cache for price of product
        using var scope = Program.ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new RefreshCacheProductPriceCommand());
    }

    public static void InitiateMemoryQueue()
    {
        OrderEventWorkers = Enumerable.Range(1, 10)
            .Select(i => new EventWorker(i.ToString(), OrderEventChannel.Reader)).ToList();
    }

}




