using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyOrderService.Data.Entities;

public class Order
{
    public string CustomerName;
    public string ProductName;
    public int Quantity;
    public double Price;

    public const string TABLE_NAME = "Orders";

    public class Configuration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(TABLE_NAME);

        }
    }
}
