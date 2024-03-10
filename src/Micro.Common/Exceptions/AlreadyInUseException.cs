namespace Micro.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AlreadyInUseException(string name, string value) : PlatformException($"{name} '{value}' is already in use");
}