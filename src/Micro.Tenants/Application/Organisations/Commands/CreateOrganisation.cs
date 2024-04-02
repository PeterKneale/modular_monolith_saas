using Micro.Tenants.Domain.OrganisationAggregate;

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

    public class Handler(IExecutionContext context, IOrganisationRepository organisations, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = new OrganisationId(command.OrganisationId);
            var userId = context.UserId;

            if (await organisations.GetAsync(organisationId, token) != null) throw new AlreadyExistsException(nameof(Organisation), organisationId.Value);

            var name = OrganisationName.Create(command.Name);
            if (await check.AnyOrganisationUsesNameAsync(name, token)) throw new AlreadyInUseException(nameof(OrganisationName), name.Value);

            var organisation = Organisation.Create(organisationId, name, userId);
            await organisations.CreateAsync(organisation, token);
        }
    }
}