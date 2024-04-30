namespace Micro.Translations.Application.Queries;

public static class GetTerm
{
    public record Query(Guid TermId) : IRequest<Result>;

    public record Result(string Name);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
        }
    }

    private class Handler(IDbConnection db) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var termId = query.TermId;
            const string sql = "select name from translate.terms where id = @termId";
            var command = new CommandDefinition(sql, new { termId }, cancellationToken: token);
            var name = await db.ExecuteScalarAsync<string>(command);
            return new Result(name);
        }
    }
}