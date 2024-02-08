using Micro.Common.Domain;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Auth;

public static class Register
{
    public record Command(Guid OrganisationId, string Name, Guid UserId, string FirstName, string LastName, string Email, string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OrganisationId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(50);
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IOrganisationRepository organisations, IUserRepository users, IOrganisationNameCheck check) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var organisationId = new OrganisationId(command.OrganisationId);
            if (await organisations.GetAsync(organisationId) != null)
            {
                throw new Exception("Organisation already exists");
            }
            
            var organisationName = new OrganisationName(command.Name);
            if (await check.AnyOrganisationUsesNameAsync(organisationName))
            {
                throw new Exception("Organisation already exists");
            }
            
            var organisation = new Organisation(organisationId, organisationName);
            await organisations.CreateAsync(organisation);
            
            var userId = new UserId(command.UserId);
            if (await users.GetAsync(userId) != null)
            {
                throw new Exception("User already exists");
            }
            
            var userName = new UserName(command.FirstName, command.LastName);
            var userEmail = command.Email;
            var userPassword = command.Password;
            var userCredentials = new UserCredentials(userEmail, userPassword);
            var user = new User(organisationId, userId, userName, userCredentials, UserRole.Admin);
            await users.CreateAsync(user);

            return Unit.Value;
        }
    }
}