using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application.Organisations;

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

    public class Handler(IUserContext context, IOrganisationRepository organisations, IMembershipRepository memberships, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = new OrganisationId(command.OrganisationId);

            if (await organisations.GetAsync(organisationId) != null)
            {
                throw new Exception("Organisation already exists");
            }

            var name = new OrganisationName(command.Name);
            if (await check.AnyOrganisationUsesNameAsync(name))
            {
                throw new Exception("Name already in use");
            }

            var organisation = new Organisation(organisationId, name);
            await organisations.CreateAsync(organisation);

            var membershipId = new MembershipId(Guid.NewGuid());
            var membership = new Membership(membershipId, organisationId, context.UserId, MembershipRole.Owner);
            await memberships.CreateAsync(membership);
            
            return Unit.Value;
        }
    }
}