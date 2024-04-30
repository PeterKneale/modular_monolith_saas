namespace Micro.Tenants.Application.Organisations.Queries;

public static class GetOrganisationByName
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

    public class Handler(IOrganisationRepository organisations) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var name = OrganisationName.Create(query.Name);
            var organisation = await organisations.GetAsync(name, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), name.Value);

            return new Result(organisation.OrganisationId.Value, organisation.Name.Value);
        }
    }
}