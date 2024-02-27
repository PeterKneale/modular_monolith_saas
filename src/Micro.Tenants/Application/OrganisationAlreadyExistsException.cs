using Micro.Common.Exceptions;

namespace Micro.Tenants.Application;

public class OrganisationAlreadyExistsException : PlatformException
{
    public OrganisationAlreadyExistsException(OrganisationId id) : base($"Organisation not found {id}")
    {
    }
}