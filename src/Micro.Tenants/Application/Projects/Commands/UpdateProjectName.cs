using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects.Commands;

public static class UpdateProjectName
{
    public record Command(Guid ProjectId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.ProjectId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IProjectRepository projects, IProjectNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = new ProjectId(command.ProjectId);

            var project = await projects.GetAsync(projectId, token);
            if (project == null)
            {
                throw new Exception("Project not found");
            }

            var name = new ProjectName(command.Name);
            if (await check.AnyOtherProjectUsesNameAsync(projectId, name, token))
            {
                throw new Exception("Name already in use");
            }

            project.ChangeName(name);
            await projects.UpdateAsync(project, token);
            return Unit.Value;
        }
    }
}