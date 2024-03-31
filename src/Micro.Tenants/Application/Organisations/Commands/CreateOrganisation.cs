using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;

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

    public class Handler(IExecutionContext context, IOrganisationRepository organisations, IMembershipRepository memberships, IUsersRepository users, IOrganisationNameCheck check , ILogger<Handler> logs) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = new OrganisationId(command.OrganisationId);

            if (await organisations.GetAsync(organisationId, token) != null) throw new AlreadyExistsException(nameof(Organisation), organisationId.Value);

            var name = OrganisationName.Create(command.Name);
            if (await check.AnyOrganisationUsesNameAsync(name, token)) throw new AlreadyInUseException(nameof(OrganisationName), name.Value);

            var organisation = Organisation.Create(organisationId, name);
            await organisations.CreateAsync(organisation, token);

            var membershipId = new MembershipId(Guid.NewGuid());
            var userId = context.UserId;
            var user = await users.GetAsync(userId, token);
            if (user == null)
            {
                logs.LogWarning($"User {userId} not found yet,possible eventual consistency issue");
            }
            else
            {
                logs.LogWarning($"User {userId} found, {user.Name}");
            }

            var membership = Membership.CreateInstance(membershipId, organisationId, userId, MembershipRole.Owner);
            await memberships.CreateAsync(membership, token);
        }
    }
}