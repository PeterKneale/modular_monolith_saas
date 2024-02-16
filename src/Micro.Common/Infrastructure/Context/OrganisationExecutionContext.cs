using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class OrganisationExecutionContext(OrganisationId organisationId) : IOrganisationExecutionContext
{
    public OrganisationId OrganisationId => organisationId;
}