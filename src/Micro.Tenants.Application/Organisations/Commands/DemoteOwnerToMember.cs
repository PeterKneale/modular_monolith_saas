namespace Micro.Tenants.Application.Organisations.Commands;

public static class DemoteOwnerToMember
{
    public record Command(Guid OwnerId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OwnerId).NotEmpty();
        }
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var ownerId = UserId.Create(command.OwnerId);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.DemoteOwnerToMember(ownerId);
            organisations.Update(organisation);
        }
    }
}