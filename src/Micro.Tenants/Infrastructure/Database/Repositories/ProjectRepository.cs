using Micro.Tenants.Application.Projects;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class ProjectRepository(Db db) : IProjectRepository
{
    public async Task CreateAsync(Project project, CancellationToken token)
    {
        await db.Projects.AddAsync(project, token);
    }

    public void Update(Project project)
    {
        db.Projects.Update(project);
    }

    public async Task<Project?> GetAsync(ProjectId id, CancellationToken token)
    {
        return await db.Projects
            .Include(x => x.Organisation)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<Project?> GetAsync(ProjectName name, CancellationToken token)
    {
        // TODO: name isnt unique , multiple tenants can have same project name
        return await db.Projects
            .Include(x => x.Organisation)
            .SingleOrDefaultAsync(x => x.Name == name, token);
    }
}