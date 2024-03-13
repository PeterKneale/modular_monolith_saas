using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects.Commands;

public static class CreateProject
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

    public class Handler(IExecutionContext context, IProjectRepository projects, IProjectNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = new ProjectId(command.ProjectId);

            if (await projects.GetAsync(projectId, token) != null) throw new AlreadyExistsException(nameof(Project), projectId.Value);

            var name = ProjectName.CreateInstance(command.Name);
            if (await check.AnyProjectUsesNameAsync(name, token)) throw new AlreadyInUseException(nameof(ProjectName), name.Value);

            var project = new Project(projectId, context.OrganisationId, name);
            await projects.CreateAsync(project, token);

            return Unit.Value;
        }
    }
}