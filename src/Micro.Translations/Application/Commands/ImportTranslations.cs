using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class ImportTranslations
{
    private const int MaxItems = 1000;

    public record Command(string LanguageCode, IDictionary<string, string> Translations) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Translations).NotEmpty().Must(x => x.Count <= MaxItems);
        }
    }

    public class Handler(ITermRepository repository, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var language = Language.FromIsoCode(command.LanguageCode);
            var termsThatExist = (await repository.ListAsync(projectId, token)).ToList();
            foreach (var item in command.Translations)
            {
                var term = termsThatExist.SingleOrDefault(x => x.Name.Value == item.Key);
                var text = TranslationText.Create(item.Value);

                if (term == null)
                {
                    var name = TermName.Create(item.Key);
                    term = Term.Create(TermId.Create(Guid.NewGuid()), projectId, name);
                    term.AddTranslation(language, text);
                    await repository.CreateAsync(term, token);
                }
                else
                {
                    term.UpdateTranslation(language, text);
                }
            }

            return Unit.Value;
        }
    }
}