using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class AddTranslation
{
    public record Command(Guid TermId, Guid LanguageId, string Text) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageId).NotEmpty();
            RuleFor(m => m.Text).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITermRepository terms) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var termId = TermId.Create(command.TermId);
            var languageId = LanguageId.Create(command.LanguageId);

            var term = await terms.GetAsync(termId, token);
            if (term == null) throw new NotFoundException(nameof(term), termId.Value);

            var text = TranslationText.Create(command.Text);
            term.AddTranslation(languageId, text);

            terms.Update(term);
        }
    }
}