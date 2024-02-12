using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects;

public interface IProjectRepository
{
    Task CreateAsync(Project project, CancellationToken token);
    Task UpdateAsync(Project project, CancellationToken token);
    Task<Project?> GetAsync(ProjectId id, CancellationToken token);
    Task<Project?> GetAsync(ProjectName name, CancellationToken token);
}