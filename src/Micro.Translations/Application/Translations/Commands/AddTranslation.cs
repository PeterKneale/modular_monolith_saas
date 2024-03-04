using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations.Commands;

public static class AddTranslation
{
    public record Command(Guid Id, Guid TermId, Guid LanguageId, string Text) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageId).NotEmpty();
            RuleFor(m => m.Text).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITranslationRepository repo) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var translationId = new TranslationId(command.Id);
            if (await repo.GetAsync(translationId, token) != null) throw new AlreadyExistsException(translationId);

            var termId = new TermId(command.TermId);
            var languageId = new LanguageId(command.LanguageId);
            if (await repo.GetAsync(termId, languageId, token) != null) throw new AlreadyExistsException(translationId);

            var text = new TranslationText(command.Text);
            var translation = new Translation(translationId, termId, languageId, text);

            await repo.CreateAsync(translation, token);
            return Unit.Value;
        }
    }
}