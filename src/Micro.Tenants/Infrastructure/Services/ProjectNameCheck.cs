using Micro.Tenants.Application.Projects;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Infrastructure.Services;

public class ProjectNameCheck(IProjectRepository repo) : IProjectNameCheck
{
    public async Task<bool> AnyProjectUsesNameAsync(ProjectName name, CancellationToken token) => await repo.GetAsync(name, token) != null;

    public async Task<bool> AnyOtherProjectUsesNameAsync(ProjectId id, ProjectName name, CancellationToken token)
    {
        var organisation = await repo.GetAsync(name, token);
        if (organisation == null)
            // no organisation uses this name
            return false;

        // an organisation uses this name, but is it the same organisation?
        var same = organisation.Id.Value == id.Value;
        return !same;
    }
}