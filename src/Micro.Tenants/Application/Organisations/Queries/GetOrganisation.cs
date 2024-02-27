using Micro.Common.Application;

namespace Micro.Tenants.Application.Organisations.Queries;

public static class GetOrganisation
{
    public record Query : IRequest<Result>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
        
    }

    public class Handler(IOrganisationExecutionContext executionContext, IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var organisationId = executionContext.OrganisationId;
            
            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null)
            {
                throw new OrganisationNotFoundException(organisationId);
            }

            return new Result(organisation.Id.Value, organisation.Name.Value);
        }
    }
}