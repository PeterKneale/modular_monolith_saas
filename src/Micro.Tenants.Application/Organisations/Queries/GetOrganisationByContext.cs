namespace Micro.Tenants.Application.Organisations.Queries;

public static class GetOrganisationByContext
{
    public record Query : IRequest<Result>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            return new Result(organisation.OrganisationId.Value, organisation.Name.Value);
        }
    }
}