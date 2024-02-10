using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations;

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
            var id = new TranslationId(command.Id);

            var translation = await translations.GetAsync(id, token);
            if (translation == null)
            {
                throw new Exception("Not found");
            }

            var text = new Text(command.Text);
            translation.UpdateText(text);

            await translations.UpdateAsync(translation, token);
            return Unit.Value;
        }
    }
}