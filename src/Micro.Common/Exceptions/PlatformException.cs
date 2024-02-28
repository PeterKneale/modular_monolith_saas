namespace Micro.Common.Exceptions;

public class PlatformException : Exception
{
    protected PlatformException(string message) : base(message)
    {
    }
}