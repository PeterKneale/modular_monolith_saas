using Micro.Translations.Domain;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations;

public static class CreateTranslation
{
    public record Command(Guid Id, Guid TermId, string Language, string Text) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.Language).NotEmpty().MaximumLength(100);
            RuleFor(m => m.Text).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITranslationRepository translations) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var id = new TranslationId(command.Id);
            var termId = new TermId(command.TermId);
            var language = Language.FromName(command.Language);
            var text = new Text(command.Text);

            var translation = new Translation(id, termId, language, text);
            await translations.CreateAsync(translation, token);
            return Unit.Value;
        }
    }
}