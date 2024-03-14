using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class DeleteOrganisation
{
    public record Command : IRequest;

    public class Validator : AbstractValidator<Command>
    {
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisations.Delete(organisation);

            
        }
    }
}