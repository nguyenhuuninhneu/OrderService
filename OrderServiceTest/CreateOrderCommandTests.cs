using FluentAssertions;
using LegacyOrderService.Config.Exceptions;
using LegacyOrderService.Data.Entities;
using LegacyOrderService.Features;
using MediatR;
using Moq;
using OrderServiceTest.Hepler;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OrderServiceTest;


public class CreateOrderCommandTests
{
    private readonly OrderDbContext _dbContext;
    private readonly Mock<IMediator> _mediatorMock;

    public CreateOrderCommandTests()
    {
        _dbContext = DbContextHelper.CreateInMemoryDbContext();
        _mediatorMock = new Mock<IMediator>();

        Program.PriceOfProducts = new ConcurrentDictionary<string, double>()
        {
            ["Widget"] = 12.99,
            ["Gadget"] = 15.49,
            ["Doohickey"] = 8.75
        };
        Program.OrderEventChannel = Channel.CreateUnbounded<object>();
    }

    [Fact]
    public async Task Handle_Should_Queue_Event_When_Valid()
    {
        // Arrange
        var handler = new CreateOrderCommand.CreateOrderCommandHandler(_dbContext, _mediatorMock.Object);

        var command = new CreateOrderCommand
        {
            CustomerName = "John Doe",
            ProductName = "Widget",
            Quantity = 2
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();

        var queued = await Program.OrderEventChannel.Reader.ReadAsync();
        if (queued is EventCreateOrderCommand convertedData)
        {
            convertedData.CustomerName.Should().Be("John Doe");
            convertedData.ProductName.Should().Be("Widget");
            convertedData.Quantity.Should().Be(2);
        }
        else
        {
            Assert.Fail("Queued message is not of type EventCreateOrderCommand");
        }
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ProductName_Missing()
    {
        var handler = new CreateOrderCommand.CreateOrderCommandHandler(_dbContext, _mediatorMock.Object);

        var command = new CreateOrderCommand
        {
            CustomerName = "John Doe",
            ProductName = "",
            Quantity = 2
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() =>
    handler.Handle(command, CancellationToken.None)
);

        // Assert
        ex.Should().Be(Exceptions.ProductNameIsRequired);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ProductName_Invalid()
    {
        var handler = new CreateOrderCommand.CreateOrderCommandHandler(_dbContext, _mediatorMock.Object);

        var command = new CreateOrderCommand
        {
            CustomerName = "John Doe",
            ProductName = "UnknownProduct",
            Quantity = 2
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() =>
    handler.Handle(command, CancellationToken.None)
);

        // Assert
        ex.Should().Be(Exceptions.ProductDoesNotExist);
    }
}

