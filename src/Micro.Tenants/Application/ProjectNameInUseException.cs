using Micro.Common.Exceptions;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class ProjectNameInUseException(ProjectName name) : PlatformException($"Project name in use {name}");