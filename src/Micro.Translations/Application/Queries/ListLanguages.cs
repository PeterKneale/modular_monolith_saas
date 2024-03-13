using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Queries;

public static class ListLanguages
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
            var languages = cultures.Select(x => Language.FromIsoCode(x.Name));

            // todo remove used

            return Task.FromResult(languages.Select(x => new Result(x.Code, x.Name)));
        }
    }
}