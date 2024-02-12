namespace Micro.Tenants.Application.Organisations;

public static class GetOrganisationById
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

    public class Handler(IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var id = new OrganisationId(query.Id);
            var organisation = await organisations.GetAsync(id, token);
            if (organisation == null)
            {
                throw new Exception("not found");
            }

            return new Result(organisation.Id.Value, organisation.Name.Value);
        }
    }
}