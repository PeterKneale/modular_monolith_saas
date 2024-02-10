using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class OrganisationContext(OrganisationId organisationId) : IOrganisationContext
{
    public OrganisationId OrganisationId => organisationId;
}