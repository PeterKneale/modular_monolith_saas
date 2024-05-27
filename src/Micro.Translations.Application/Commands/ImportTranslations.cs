using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class ImportTranslations
{
    private const int MaxItems = 1000;

    public record Command(Guid LanguageId, IDictionary<string, string> Translations) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.LanguageId).NotEmpty();
            RuleFor(m => m.Translations).NotEmpty().Must(x => x.Count <= MaxItems);
        }
    }

    public class Handler(ITermRepository repository, ILanguageRepository languages, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = LanguageId.Create(command.LanguageId);
            var language = await languages.GetAsync(languageId, token);
            if (language == null) throw new NotFoundException(nameof(Language), languageId.Value);

            var termsThatExist = (await repository.ListAsync(projectId, token)).ToList();
            foreach (var item in command.Translations)
            {
                var term = termsThatExist.SingleOrDefault(x => x.Name.Value == item.Key);
                var text = TranslationText.Create(item.Value);

                if (term == null)
                {
                    var name = TermName.Create(item.Key);
                    term = Term.Create(TermId.Create(Guid.NewGuid()), projectId, name);
                    term.AddTranslation(languageId, text);
                    await repository.CreateAsync(term, token);
                }
                else
                {
                    term.UpdateTranslation(languageId, text);
                }
            }
        }
    }
}