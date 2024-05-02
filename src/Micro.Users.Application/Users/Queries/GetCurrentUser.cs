using System.Data;
using Dapper;

namespace Micro.Users.Application.Users.Queries;

public static class GetCurrentUser
{
    public record Query : IRequest<Result>;
    
    public record Result(Guid Id, string FirstName, string LastName);

    public class Validator : AbstractValidator<Query>;
    
    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var userId = context.UserId;
            
            const string sql = """
                               SELECT id, first_name, last_name
                               FROM users
                               WHERE id = @id
                               """;
            var command = new CommandDefinition(sql, new { id = userId.Value }, cancellationToken: token);
            var result = await db.QuerySingleOrDefaultAsync<Result?>(command);
            if (result == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            return result;
        }
    }
}