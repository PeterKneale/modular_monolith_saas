using Micro.Common.Exceptions;

namespace Micro.Common.Domain;

[ExcludeFromCodeCoverage]
public class BusinessRuleBrokenException(string message) : PlatformException(message);