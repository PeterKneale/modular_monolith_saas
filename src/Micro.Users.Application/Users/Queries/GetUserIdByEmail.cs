using System.Data;
using Dapper;

namespace Micro.Users.Application.Users.Queries;

public static class GetUserIdByEmail
{
    public record Query(string Email) : IRequest<Guid>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class Handler(IDbConnection db) : IRequestHandler<Query, Guid>
    {
        public async Task<Guid> Handle(Query query, CancellationToken token)
        {
            var email = EmailAddress.Create(query.Email);

            const string sql = "SELECT id FROM users WHERE email_canonical = @Email";
            var command = new CommandDefinition(sql, new { Email = email.Canonical }, cancellationToken: token);
            var userId = await db.QuerySingleOrDefaultAsync<Guid?>(command);
            if (!userId.HasValue) throw new NotFoundException(nameof(User), email);

            return userId.Value;
        }
    }
}