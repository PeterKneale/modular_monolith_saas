using Micro.Common.Exceptions;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application;

public class OrganisationNameInUseException : PlatformException
{
    public OrganisationNameInUseException(OrganisationName name) : base($"Organisation name in use {name}")
    {
    }
}