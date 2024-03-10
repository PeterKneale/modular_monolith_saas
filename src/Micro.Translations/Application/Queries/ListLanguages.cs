using System.Globalization;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(string Code, string Name);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var langauge = cultures.Select(x => Language.FromIsoCode(x.Name));
            
            // todo remove used
            
            return langauge.Select(x => new Result(x.Code, x.Name));
        }
    }
}