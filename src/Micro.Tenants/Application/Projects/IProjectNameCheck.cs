using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects;

public interface IProjectNameCheck
{
    Task<bool> AnyProjectUsesNameAsync(ProjectName name, CancellationToken token);

    Task<bool> AnyOtherProjectUsesNameAsync(ProjectId id, ProjectName name, CancellationToken token);
}