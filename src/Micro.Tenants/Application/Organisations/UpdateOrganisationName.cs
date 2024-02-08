using Micro.Common.Application;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

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

    public class Handler(ICurrentContext context, IOrganisationRepository organisations, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;
            
            var organisation = await organisations.GetAsync(organisationId);
            if(organisation == null)
            {
                throw new Exception("Organisation not found");
            }

            var name = new OrganisationName(command.Name);
            if(await check.AnyOtherOrganisationUsesNameAsync(organisationId, name))
            {
                throw new Exception("Name already in use");
            }
            organisation.ChangeName(name);
            await organisations.UpdateAsync(organisation);
            return Unit.Value;
        }
    }
}