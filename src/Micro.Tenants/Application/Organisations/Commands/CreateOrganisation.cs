using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class CreateOrganisation
{
    public record Command(Guid OrganisationId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OrganisationId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IUserExecutionContext executionContext, IOrganisationRepository organisations, IMembershipRepository memberships, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = new OrganisationId(command.OrganisationId);

            if (await organisations.GetAsync(organisationId, token) != null) throw new AlreadyExistsException(nameof(Organisation), organisationId.Value);

            var name = OrganisationName.Create(command.Name);
            if (await check.AnyOrganisationUsesNameAsync(name, token)) throw new AlreadyInUseException(nameof(OrganisationName), name.Value);

            var organisation = Organisation.Create(organisationId, name);
            await organisations.CreateAsync(organisation, token);

            var membershipId = new MembershipId(Guid.NewGuid());
            var membership = Membership.CreateInstance(membershipId, organisationId, executionContext.UserId, MembershipRole.Owner);
            await memberships.CreateAsync(membership, token);

            return Unit.Value;
        }
    }
}