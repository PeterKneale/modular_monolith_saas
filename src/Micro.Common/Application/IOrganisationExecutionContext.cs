using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IOrganisationExecutionContext
{
    OrganisationId OrganisationId { get; }
}