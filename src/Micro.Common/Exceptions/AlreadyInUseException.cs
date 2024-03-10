namespace Micro.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AlreadyInUseException(string name, string value) : PlatformException($"{name} '{name}' is already in use");
}