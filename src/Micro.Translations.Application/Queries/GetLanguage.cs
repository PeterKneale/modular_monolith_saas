﻿using System.Globalization;

namespace Micro.Translations.Application.Queries;

public static class GetLanguage
{
    public record Query(string LanguageCode) : IRequest<Result>;

    public record Result(Guid Id, string Code, string Name);

    public class Validator : AbstractValidator<Query>;

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageCode = query.LanguageCode;

            var sql = """
                      SELECT id
                      FROM translate.languages
                      WHERE project_id = @projectId AND language_code = @languageCode
                      """;

            var command = new CommandDefinition(sql, new { projectId, languageCode }, cancellationToken: token);
            var id = await db.ExecuteScalarAsync<Guid>(command);
            var name = CultureInfo.GetCultureInfo(languageCode).DisplayName;
            return new Result(id, languageCode, name);
        }
    }
}