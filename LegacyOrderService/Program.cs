using LegacyOrderService.Config.Exceptions;
using LegacyOrderService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;


//PRepare everything before start the application


var services = new ServiceCollection();

services.AddDbContext<OrderDbContext>(opt =>
    opt.UseSqlite(@"Data Source=Data\orders.db"));

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});


