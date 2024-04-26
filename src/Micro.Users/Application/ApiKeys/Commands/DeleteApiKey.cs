namespace Micro.Users.Application.ApiKeys.Commands;

public static class DeleteApiKey
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
        public async Task Handle(Command command, CancellationToken token)
        {
            var id = new UserApiKeyId(command.Id);

            var key = await keys.GetById(id, token);

            if (key == null) throw new NotFoundException(nameof(UserApiKey), id.Value);

            keys.Delete(key);
        }
    }
}