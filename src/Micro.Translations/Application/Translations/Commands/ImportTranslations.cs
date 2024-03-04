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

    public class Handler(ILanguageRepository languageRepo, ITermRepository termRepo, ITranslationRepository translationRepo, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var languageId = await GetOrCreateLanguage(command.LanguageCode, token);

            var termNames = command.Translations.Select(x => x.Key);

            var termsToImport = termNames.Select(x => Term.Create(x, projectId)).ToList();
            var termsThatExist = (await termRepo.ListAsync(projectId, token)).ToList();
            var termsToCreate = (termsToImport.Except(termsThatExist)).ToList();

            foreach (var term in termsToCreate)
                await termRepo.CreateAsync(term, token);

            foreach (var item in command.Translations)
            {
                var text = new TranslationText(item.Value);

                // Get term by name (needs to use the term id as i think the unsaved term name canot be found)
                var term = termsThatExist.SingleOrDefault(x => x.Name.Value == item.Key);
                if (term == null)
                {
                    term = termsToCreate.SingleOrDefault(x => x.Name.Value == item.Key);
                    if (term == null)
                    {
                        throw new InvalidOperationException($"Term '{item.Key}' not found");
                    }
                }

                // Check for a translation
                var existing = await translationRepo.GetAsync(term.Id, languageId, token);
                if (existing != null)
                {
                    existing.UpdateText(text);
                    translationRepo.Update(existing);
                }
                else
                {
                    var translation = term.CreateTranslation(languageId, text);
                    await translationRepo.CreateAsync(translation, token);
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