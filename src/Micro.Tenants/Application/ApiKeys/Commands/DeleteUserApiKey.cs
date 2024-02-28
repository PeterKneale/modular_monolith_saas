using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys.Commands;

public static class DeleteUserApiKey
{
    public record Command(Guid Id) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IApiKeyRepository keys) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var id = new UserApiKeyId(command.Id);
            
            if (await keys.GetById(id, token) == null)
            {
                throw new ApiKeyNotFoundException(id);
            }

            await keys.DeleteAsync(id, token);
            
            return Unit.Value;
        }
    }
}