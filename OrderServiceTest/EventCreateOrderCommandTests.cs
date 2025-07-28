using FluentAssertions;
using LegacyOrderService.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using OrderServiceTest.Hepler;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OrderServiceTest
{
    public class EventCreateOrderCommandTests
    {
        private readonly OrderDbContext _dbContext;
        private readonly Mock<IMediator> _mediatorMock;

        public EventCreateOrderCommandTests()
        {
            _dbContext = DbContextHelper.CreateInMemoryDbContext();
            _mediatorMock = new Mock<IMediator>();

            Program.PriceOfProducts = new ConcurrentDictionary<string, double>()
            {
                ["Widget"] = 12.99,
                ["Gadget"] = 15.49,
                ["Doohickey"] = 8.75
            };
        }
        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateOrder()
        {

            var command = new EventCreateOrderCommand
            {
                CustomerName = "Bob",
                ProductName = "Doohickey",
                Quantity = 3
            };

            var handler = new EventCreateOrderCommand.EventCreateOrderCommandHandler(_dbContext);

            // Act
            var createOrder = await handler.Handle(command, CancellationToken.None);

            // Assert
            createOrder.Should().Be(true);
        }

        [Fact]
        public async Task Handle_MultipleValidCommands_ShouldCreateAllOrders()
        {
            // Arrange
            var handler = new EventCreateOrderCommand.EventCreateOrderCommandHandler(_dbContext);

            int numberRequestSuccess = 10;

            var commands = Enumerable.Range(1, numberRequestSuccess).Select(i => new EventCreateOrderCommand
            {
                CustomerName = $"Bob_{i}",
                ProductName = $"Doohickey",
                Quantity = i
            }).ToList();

            // Act
            var results = await Task.WhenAll(commands.Select(cmd => handler.Handle(cmd, CancellationToken.None)));

            // Assert

            results.Count(p=>p).Should().Be(numberRequestSuccess);
        }

    }

}