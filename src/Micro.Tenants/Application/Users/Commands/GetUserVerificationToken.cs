using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users.Commands;

public static class GetUserVerificationToken
{
    public record Query(Guid UserId) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    private class Handler(IUserRepository users) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            var userId = new UserId(query.UserId);
            var user = await users.GetAsync(userId, cancellationToken);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);

            if (user.Verification.IsVerified) throw new InvalidOperationException(userId);

            return user.Verification.VerificationToken!;
        }
    }
}