using Micro.Common.Exceptions;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class OrganisationAlreadyExistsException(OrganisationId id) : PlatformException($"Organisation already exists {id}");