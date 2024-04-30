namespace Micro.Tenants.Application.Organisations.Queries;

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

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var organisationId = context.OrganisationId;
            var projectName = ProjectName.Create(query.Name);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            var project = organisation.Projects.SingleOrDefault(x => x.Name.Equals(projectName));
            if (project == null) throw new NotFoundException(nameof(Project), projectName.Value);

            return new Result(project.ProjectId.Value, project.Name.Value);
        }
    }
}