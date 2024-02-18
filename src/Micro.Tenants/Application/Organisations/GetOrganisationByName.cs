﻿using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

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
            var name = new OrganisationName(query.Name);
            var organisation = await organisations.GetAsync(name, token);
            if (organisation == null)
            {
                throw new Exception($"organisation not found {name}");
            }

            return new Result(organisation.Id.Value, organisation.Name.Value);
        }
    }
}