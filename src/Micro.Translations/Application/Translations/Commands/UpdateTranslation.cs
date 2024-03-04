using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations.Commands;

public static class UpdateTranslation
{
    public record Command(Guid Id, string Text) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Text).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITranslationRepository translations) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var translationId = new TranslationId(command.Id);

            var translation = await translations.GetAsync(translationId, token);
            if (translation == null)
            {
                throw new NotFoundException(translationId);
            }

            var text = new TranslationText(command.Text);
            translation.UpdateText(text);

            translations.Update(translation);
            return Unit.Value;
        }
    }
}