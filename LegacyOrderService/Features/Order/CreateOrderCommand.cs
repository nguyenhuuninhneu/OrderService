using LegacyOrderService.Config.Exceptions;
using LegacyOrderService.Data.Entities;
using LegacyOrderService.Helpers;
using MediatR;
using System;

namespace LegacyOrderService.Features;

public class CreateOrderCommand : IRequest<string>
{
    public string CustomerName { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }


    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext, IMediator mediator)
        {
            _orderDbContext = orderDbContext;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {

            //validate data
            command.ProductName.IsEmpty().ThenThrow(Exceptions.ProductNameIsRequired);

            command.CustomerName.IsEmpty().ThenThrow(Exceptions.CustomerNameIsRequired);

            (command.Quantity <= 0).ThenThrow(Exceptions.QuantityMustBeGreaterThanZero);

            (!Program.PriceOfProducts.ContainsKey(command.ProductName)).ThenThrow(Exceptions.ProductDoesNotExist);

            //push message to queue
            await Program.OrderEventChannel.Writer.WriteAsync(new EventCreateOrderCommand()
            {
                CustomerName = command.CustomerName,
                ProductName = command.ProductName,
                Quantity = command.Quantity,
            });

            Console.WriteLine("Order queued. Will be processed shortly...");

            return "";

        }
    }
}
