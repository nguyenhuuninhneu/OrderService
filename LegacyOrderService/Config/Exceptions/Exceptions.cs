

namespace LegacyOrderService.Config.Exceptions;

public static class Exceptions
{
    public static DomainException ProductIdIsRequired = new DomainException("ProjectId is required");
    public static DomainException ProductNameIsRequired = new DomainException("Product name is required");
    public static DomainException CustomerNameIsRequired = new DomainException("Customer name is required");
    public static DomainException QuantityMustBeGreaterThanZero = new DomainException("Quantity must be greater than zero");
    public static DomainException ProductDoesNotExist = new DomainException("Product does not exist");

    public static DomainException AppendMessage(this DomainException exception, string message)
    {
        var newMessage = $"{exception.Message}: {message}";
        return new DomainException(exception.Code, newMessage);
    }
}