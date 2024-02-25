using Micro.Common.Application;
using Micro.Tenants.Application.Users;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys.Commands;

public static class CreateUserApiKey
{
    public record Command(Guid Id, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IUserExecutionContext context, IApiKeyRepository keys, IApiKeyService service) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var id = new UserApiKeyId(command.Id);
            var name = new ApiKeyName(command.Name);
            var userId = context.UserId;

            if (await keys.GetAsync(userId, id, token) != null)
            {
                throw new Exception("User api key already exists");
            }

            if (await keys.GetAsync(userId, name, token) != null)
            {
                throw new Exception($"Name {name} already in use");
            }

            var key = UserApiKey.CreateNew(id, userId, name, service);
            await keys.CreateAsync(key, token);

            return Unit.Value;
        }
    }
}