

namespace LegacyOrderService.Config.Exceptions;

public static class Exceptions
{
    public static DomainException ProductIdIsRequired = new DomainException("ProjectId is required");
    public static DomainException ProductNameIsRequired = new DomainException("ProductName is required");

    public static DomainException AppendMessage(this DomainException exception, string message)
    {
        var newMessage = $"{exception.Message}: {message}";
        return new DomainException(exception.Code, newMessage);
    }
}