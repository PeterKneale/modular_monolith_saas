using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Application.Queries;

public static class ListAllLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(string Code, string Name);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        public Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var languages = cultures.Select(x => LanguageDetail.Create(x.Name));

            // todo remove used

            return Task.FromResult(languages.Select(x => new Result(x.Code, x.Name)));
        }
    }
}