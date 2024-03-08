using Micro.Common.Application;
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

    public class Handler(IProjectRepository projects, IProjectNameCheck check, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

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
            projects.Update(project);
            return Unit.Value;
        }
    }
}