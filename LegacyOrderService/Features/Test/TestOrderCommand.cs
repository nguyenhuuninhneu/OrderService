using LegacyOrderService.Config.Exceptions;
using LegacyOrderService.Data.Entities;
using LegacyOrderService.Helpers;
using MediatR;
using System;

namespace LegacyOrderService.Features;

public class TestOrderCommand : IRequest
{


    public class CreateOrderCommandHandler : IRequestHandler<TestOrderCommand>
    {
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(TestOrderCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("Enter customer name:");
            var name = Console.ReadLine();

            Console.WriteLine("Enter product name:");
            var product = Console.ReadLine();

            Console.WriteLine("Enter quantity:");
            if (!int.TryParse(Console.ReadLine(), out var qty))
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            var newOrder = new CreateOrderCommand
            {
                CustomerName = name!,
                ProductName = product!,
                Quantity = qty
            };

            await _mediator.Send(newOrder);

        }
    }
}
