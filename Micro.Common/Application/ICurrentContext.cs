using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface ICurrentContext
{
    UserId UserId { get; }
    OrganisationId OrganisationId { get; }
}