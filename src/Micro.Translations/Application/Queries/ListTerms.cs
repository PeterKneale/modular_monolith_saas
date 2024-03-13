﻿namespace Micro.Translations.Application.Queries;

public static class ListTerms
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    private class Handler(Db db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            return await db.Terms
                .AsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .Select(x => new Result(x.Id.Value, x.Name.Value))
                .ToListAsync(token);
        }
    }
}