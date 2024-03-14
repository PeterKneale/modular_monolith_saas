using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class UpdateOrganisationName
{
    public record Command(string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IOrganisationRepository organisations, IOrganisationNameCheck check, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            var name = OrganisationName.Create(command.Name);
            if (await check.AnyOtherOrganisationUsesNameAsync(organisationId, name, token)) throw new AlreadyInUseException(nameof(OrganisationName), name.Value);

            organisation.ChangeName(name);
            organisations.Update(organisation);
        }
    }
}