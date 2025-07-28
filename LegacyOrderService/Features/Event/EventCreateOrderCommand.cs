using LegacyOrderService.Data.Entities;
using MediatR;

public class EventCreateOrderCommand : IRequest<bool>
{
    public string CustomerName { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public long Quantity { get; set; }

    public class EventCreateOrderCommandHandler : IRequestHandler<EventCreateOrderCommand, bool>
    {
        private readonly OrderDbContext _orderDbContext;

        public EventCreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<bool> Handle(EventCreateOrderCommand request, CancellationToken cancellationToken)
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

            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync(cancellationToken);

            Console.WriteLine("Order created");
            return true;
        }
    }
}
