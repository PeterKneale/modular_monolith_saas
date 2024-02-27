using Micro.Common.Exceptions;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application;

public class OrganisationNotFoundException : PlatformException
{
    public OrganisationNotFoundException(OrganisationId id) : base($"Organisation not found {id}")
    {
    }
    public OrganisationNotFoundException(OrganisationName name) : base($"Organisation not found {name}")
    {
    }
}