using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;
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

    public class Handler(ITranslationRepository translations) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var id = new TranslationId(command.Id);
            var termId = new TermId(command.TermId);
            var languageId = new LanguageId(command.LanguageId);
            var text = new TranslationText(command.Text);

            var translation = new Translation(id, termId, languageId, text);
            await translations.CreateAsync(translation, token);
            return Unit.Value;
        }
    }
}