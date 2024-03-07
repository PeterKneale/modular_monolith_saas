using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations.Commands;

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

    public class Handler(ITermRepository repository, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var language = Language.FromIsoCode(command.LanguageCode);
            var termsThatExist = (await repository.ListAsync(projectId, token)).ToList();
            foreach (var item in command.Translations)
            {
                var term = termsThatExist.SingleOrDefault(x => x.Name.Value == item.Key);
                var text = new TranslationText(item.Value);
                
                if (term == null)
                {
                    term = Term.Create(item.Key, projectId);
                    term.AddTranslation(language, text);
                    await repository.CreateAsync(term, token);
                }
                else
                {
                    term.UpdateTranslation(language,text);
                }
            }
            return Unit.Value;
        }
    }
}