namespace Micro.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class AlreadyExistsException : PlatformException
{
    public AlreadyExistsException(string name, Guid id) : base($"{name} '{id}' already exists")
    {
    }
    
    private AlreadyExistsException(string name, string value) : base($"{name} '{value}' already exists")
    {
    }
    
    public static void ThrowBecauseEmailAlreadyExists(string name, string email)
    {
        throw new AlreadyExistsException(name, email);
    }
}