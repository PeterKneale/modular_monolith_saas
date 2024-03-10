using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Application.Projects.Queries;

public static class GetProjectByName
{
    public record Query(string Name) : IRequest<Result>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IProjectRepository projects) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var name = ProjectName.CreateInstance(query.Name);

            var project = await projects.GetAsync(name, token);
            if (project == null) throw new Exception("not found");

            return new Result(project.Id.Value, project.Name.Value);
        }
    }
}