using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class UpdateMemberRole
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

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var userId = new UserId(command.UserId);
            var role = MembershipRole.FromString(command.Role);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.UpdateMembershipRole(userId, role);
            organisations.Update(organisation);
        }
    }
}