using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application.Translations.Commands;

public static class RemoveTranslation
{
    public record Command(Guid TermId, string LanguageCode) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageCode).NotEmpty();
        }
    }

    public class Handler(ITermRepository terms) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var termId = new TermId(command.TermId);
            var language = Language.FromIsoCode(command.LanguageCode);

            var term = await terms.GetAsync(termId, token);
            if (term == null) throw new NotFoundException(termId);

            term.RemoveTranslation(language);

            terms.Update(term);
            return Unit.Value;
        }
    }
}