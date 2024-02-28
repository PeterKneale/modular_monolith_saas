using Micro.Common.Exceptions;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class ProjectAlreadyExistsException(ProjectId id) : PlatformException($"Project already exists {id}");