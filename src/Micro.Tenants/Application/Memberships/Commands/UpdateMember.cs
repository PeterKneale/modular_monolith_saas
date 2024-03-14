using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application.Memberships.Commands;

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

    public class Handler(IExecutionContext context, IMembershipRepository memberships) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
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

            
        }
    }
}