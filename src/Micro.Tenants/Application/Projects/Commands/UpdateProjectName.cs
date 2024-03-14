using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects.Commands;

public static class UpdateProjectName
{
    public record Command(string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IProjectRepository projects, IProjectNameCheck check, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var project = await projects.GetAsync(projectId, token);
            if (project == null) throw new NotFoundException(nameof(Project), projectId.Value);

            var name = ProjectName.CreateInstance(command.Name);
            if (await check.AnyOtherProjectUsesNameAsync(projectId, name, token)) throw new AlreadyInUseException(nameof(ProjectName), name.Value);

            project.ChangeName(name);
            projects.Update(project);
            
        }
    }
}