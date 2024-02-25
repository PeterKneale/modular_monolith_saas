namespace Micro.Tenants.Application.Projects.Queries;

public static class GetProjectById
{
    public record Query(Guid Id) : IRequest<Result>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IProjectRepository projects) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var id = new ProjectId(query.Id);
            var project = await projects.GetAsync(id, token);
            if (project == null)
            {
                throw new Exception("not found");
            }

            return new Result(project.Id.Value, project.Name.Value);
        }
    }
}