namespace Micro.Tenants.Application.Organisations.Commands;

public static class PromoteMemberToOwner
{
    public record Command(Guid MemberId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.MemberId).NotEmpty();
        }
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var memberId = UserId.Create(command.MemberId);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.PromoteMemberToOwner(memberId);
            organisations.Update(organisation);
        }
    }
}