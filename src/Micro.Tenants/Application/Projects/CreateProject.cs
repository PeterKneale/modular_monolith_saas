using Micro.Common.Application;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects;

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

    public class Handler(IOrganisationExecutionContext executionContext, IProjectRepository projects, IProjectNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = new ProjectId(command.ProjectId);

            if (await projects.GetAsync(projectId, token) != null)
            {
                throw new Exception("Project already exists");
            }

            var name = new ProjectName(command.Name);
            if (await check.AnyProjectUsesNameAsync(name, token))
            {
                throw new Exception($"Name {name} already in use");
            }

            var project = new Project(projectId, executionContext.OrganisationId, name);
            await projects.CreateAsync(project, token);

            return Unit.Value;
        }
    }
}