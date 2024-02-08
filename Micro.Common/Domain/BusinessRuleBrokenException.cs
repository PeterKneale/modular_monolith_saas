namespace Micro.Common.Domain;

[ExcludeFromCodeCoverage]
public class BusinessRuleBrokenException : Exception
{
    public BusinessRuleBrokenException(string message)
        : base(message)
    {
    }
}