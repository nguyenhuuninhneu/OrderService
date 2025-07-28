using LegacyOrderService.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyOrderService.Features.System;

public class RefreshCacheProductPriceCommand: IRequest
{
    public class RefreshCacheProductPriceCommandHandler : IRequestHandler<RefreshCacheProductPriceCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public RefreshCacheProductPriceCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Handle(RefreshCacheProductPriceCommand command, CancellationToken cancellationToken)
        {
            //it is only mock data

            //On the production it should get data from DB instead of this and the key should be Id instead of name of product
            //every step related to create, update, delete product also change the price, it should call this command to update newest data for the cache

             Dictionary<string, double> _productPrices = new()
            {
                ["Widget"] = 12.99,
                ["Gadget"] = 15.49,
                ["Doohickey"] = 8.75
            };

            Program.PriceOfProducts.Clear();

            foreach (var item in _productPrices)
            {
                Program.PriceOfProducts.TryAdd(item.Key, item.Value);
            }
        }
    }
}
