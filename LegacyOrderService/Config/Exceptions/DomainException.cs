namespace LegacyOrderService.Config.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
        Code = "";
    }
    
    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}