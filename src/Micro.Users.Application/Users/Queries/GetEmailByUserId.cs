using System.Data;
using Dapper;

namespace Micro.Users.Application.Users.Queries;

public static class GetEmailByUserId
{
    public record Query(Guid UserId) : IRequest<Result>;

    public record Result(string Canonical, string Display);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class Handler(IDbConnection db) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var userId = UserId.Create(query.UserId);

            const string sql = """
                               SELECT email_canonical as canonical, email_display as display
                               FROM users
                               WHERE id = @id
                               """;
            var command = new CommandDefinition(sql, new { id = userId.Value }, cancellationToken: token);
            var result = await db.QuerySingleOrDefaultAsync<Result?>(command);
            if (result == null) throw new NotFoundException(nameof(User), userId);

            return result;
        }
    }
}