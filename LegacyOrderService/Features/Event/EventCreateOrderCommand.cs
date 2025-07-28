using LegacyOrderService.Data.Entities;
using MediatR;

public class EventCreateOrderCommand : IRequest
{
    public string CustomerName { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }

    public class EventCreateOrderCommandHandler : IRequestHandler<EventCreateOrderCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public EventCreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Handle(EventCreateOrderCommand request, CancellationToken cancellationToken)
        {

            //get price from cache
            Program.PriceOfProducts.TryGetValue(request.ProductName, out double price);

            var order = new Order
            {
                CustomerName = request.CustomerName,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                Price = price
            };

            _orderDbContext.Orders.Add(order);
            await _orderDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
