using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class UpdateTranslation
{
    public record Command(Guid TermId, string LanguageCode, string Text) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageCode).NotEmpty();
            RuleFor(m => m.Text).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITermRepository terms) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var termId = new TermId(command.TermId);
            var text = new TranslationText(command.Text);
            var language = Language.FromIsoCode(command.LanguageCode);

            var term = await terms.GetAsync(termId, token);
            if (term == null) throw new NotFoundException(termId);

            term.UpdateTranslation(language, text);

            terms.Update(term);
            return Unit.Value;
        }
    }
}