using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations.Commands;

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
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var termId = new TermId(command.TermId);
            
            var term = await terms.GetAsync(termId, token);
            if (term == null)
            {
                throw new NotFoundException(termId);
            }
            
            var text = new TranslationText(command.Text);
            var languageId = new LanguageId(command.LanguageId);
            term.AddTranslation(languageId, text);
            
            terms.Update(term);
            return Unit.Value;
        }
    }
}