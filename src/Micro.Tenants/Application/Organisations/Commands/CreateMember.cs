using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class CreateMember
{
    public record Command(Guid UserId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var userId = new UserId(command.UserId);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.AddMember(userId);
            organisations.Update(organisation);
        }
    }
}