using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Application.Memberships.Commands;

public static class DeleteMember
{
    public record Command(Guid UserId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler(IExecutionContext context, IMembershipRepository memberships) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var userId = new UserId(command.UserId);

            var membership = await memberships.Get(organisationId, userId, token);
            if (membership == null)
                // TODO: its identified by two ids, which one to throw - is it an aggregate root?
                throw new NotFoundException(nameof(Membership), organisationId.Value);

            memberships.Delete(membership);

            
        }
    }
}