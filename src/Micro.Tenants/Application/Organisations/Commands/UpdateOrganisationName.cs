using Micro.Common.Application;
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

    public class Handler(IOrganisationRepository organisations, IOrganisationNameCheck check, IOrganisationExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null)
            {
                throw new OrganisationNotFoundException(organisationId);
            }

            var name = new OrganisationName(command.Name);
            if (await check.AnyOtherOrganisationUsesNameAsync(organisationId, name, token))
            {
                throw new OrganisationNameInUseException(name);
            }

            organisation.ChangeName(name);
            await organisations.UpdateAsync(organisation, token);
            return Unit.Value;
        }
    }
}