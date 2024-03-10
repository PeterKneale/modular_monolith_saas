namespace Micro.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException : PlatformException
{
    public NotFoundException(string name, string id) : base($"{name} '{id}' not found")
    {
    }

    public NotFoundException(string name, Guid id) : base($"{name} '{id}' not found")
    {
    }
}