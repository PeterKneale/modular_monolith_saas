using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IOrganisationContext
{
    OrganisationId OrganisationId { get; }
}