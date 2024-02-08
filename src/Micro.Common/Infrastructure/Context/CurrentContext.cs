using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class CurrentContext(OrganisationId organisationId, UserId userId) : ICurrentContext
{
    public OrganisationId OrganisationId => organisationId;
    
    public UserId UserId => userId;
}