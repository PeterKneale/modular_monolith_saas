using Micro.Tenants.Messages;
using Micro.Translations.Domain;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration.EventHandlers;

public class ProjectCreatedHandler(Db db, ILogger<ProjectCreatedHandler> logs) : INotificationHandler<ProjectCreated>
{
    public async Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Syncing project created: {notification.ProjectName}");
        var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == notification.ProjectId, cancellationToken);
        if (project == null)
        {
            logs.LogInformation($"Syncing project changed (inserting): {notification.ProjectName}");
            await db.Projects.AddAsync(new Project
            {
                Id = notification.ProjectId,
                Name = notification.ProjectName
            }, cancellationToken);
        }
        else
        {
            logs.LogInformation($"Syncing project changed (updating): {notification.ProjectName}");
            project.Name = notification.ProjectName;
            db.Projects.Update(project);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}