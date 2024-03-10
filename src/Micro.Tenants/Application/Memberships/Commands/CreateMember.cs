using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Memberships.Commands;

public static class CreateMember
{
    public record Command(Guid UserId, string Role) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Role).NotEmpty();
        }
    }

    public class Handler(IOrganisationExecutionContext context, IOrganisationRepository organisations, IMembershipRepository memberships, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var userId = new UserId(command.UserId);
            var role = command.Role;

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            var membershipId = new MembershipId(Guid.NewGuid());
            var membership = Membership.CreateInstance(membershipId, organisationId, userId, MembershipRole.FromString(role));
            await memberships.CreateAsync(membership, token);

            return Unit.Value;
        }
    }
}

public static class UpdateMember
{
    public record Command(Guid UserId, string Role) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Role).NotEmpty();
        }
    }

    public class Handler(IOrganisationExecutionContext context, IOrganisationRepository organisations, IMembershipRepository memberships, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var userId = new UserId(command.UserId);
            var role = MembershipRole.FromString(command.Role);


            var membership = await memberships.Get(organisationId, userId, token);
            if (membership == null)
                // TODO: its identified by two ids, which one to throw - is it an aggregate root?
                throw new NotFoundException(nameof(Membership), organisationId.Value);
            membership.SetRole(role);

            memberships.Update(membership);

            return Unit.Value;
        }
    }
}