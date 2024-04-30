namespace Micro.Translations.Application.Queries;

public static class GetTranslation
{
    public record Query(Guid TermId, Guid LanguageId) : IRequest<Result>;

    public record Result(string Text);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(IDbConnection db) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var termId = query.TermId;
            var languageId = query.LanguageId;
            const string sql = "select text from translate.translations where term_id = @termId and language_id = @languageId";
            var command = new CommandDefinition(sql, new { termId, languageId }, cancellationToken: token);
            var name = await db.ExecuteScalarAsync<string>(command);
            return new Result(name);
        }
    }
}