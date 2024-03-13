using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IExecutionContext
{
    UserId UserId { get; }
    OrganisationId OrganisationId { get; }
    ProjectId ProjectId { get; }
}