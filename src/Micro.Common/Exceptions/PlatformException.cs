using Micro.Common.Domain;

namespace Micro.Common.Exceptions;

public class PlatformException:Exception
{
    protected PlatformException(string message):base(message)
    {
    }
}
public class NotFoundException(string message) : PlatformException(message);
