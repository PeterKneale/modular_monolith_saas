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

    public class Handler(ILanguageRepository languageRepo, ITermRepository termRepo, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var languageId = await GetOrCreateLanguage(command.LanguageCode, token);
            var termsThatExist = (await termRepo.ListAsync(projectId, token)).ToList();
            foreach (var item in command.Translations)
            {
                var term = termsThatExist.SingleOrDefault(x => x.Name.Value == item.Key);
                var text = new TranslationText(item.Value);
                
                if (term == null)
                {
                    term = Term.Create(item.Key, projectId);
                    term.AddTranslation(languageId, text);
                    await termRepo.CreateAsync(term, token);
                }
                else
                {
                    term.UpdateTranslation(languageId,text);
                }
            }
            return Unit.Value;
        }

        private async Task<LanguageId> GetOrCreateLanguage(string languageCode, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var code = LanguageCode.FromIsoCode(languageCode);
            var language = await languageRepo.GetAsync(projectId, code, token);
            if (language != null) return language.Id;

            var languageId = new LanguageId(Guid.NewGuid());
            language = new Language(languageId, projectId, code);
            await languageRepo.CreateAsync(language, token);
            return languageId;
        }
    }
}