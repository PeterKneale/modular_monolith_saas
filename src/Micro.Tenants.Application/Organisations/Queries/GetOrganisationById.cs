namespace Micro.Tenants.Application.Organisations.Queries;

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
            var id = OrganisationId.Create(query.Id);
            var organisation = await organisations.GetAsync(id, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), id.Value);

            return new Result(organisation.OrganisationId.Value, organisation.Name.Value);
        }
    }
}