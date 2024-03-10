namespace Micro.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class AlreadyExistsException(string name, Guid id) : PlatformException($"{name} '{id}' already exists");