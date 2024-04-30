namespace Micro.Tenants.Application.Organisations.Queries;

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

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var organisationId = context.OrganisationId;
            var projectId = ProjectId.Create(query.Id);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            var project = organisation.Projects.SingleOrDefault(x => x.ProjectId.Equals(projectId));
            if (project == null) throw new NotFoundException(nameof(Project), projectId.Value);

            return new Result(project.ProjectId.Value, project.Name.Value);
        }
    }
}