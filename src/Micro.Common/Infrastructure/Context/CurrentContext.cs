using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class CurrentContext(OrganisationId? organisationId, UserId? userId) : ICurrentContext
{
    public OrganisationId OrganisationId => organisationId ?? throw new Exception("Organisation context not available");
    
    public UserId UserId => userId ?? throw new Exception("User context not available");
}