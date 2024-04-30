using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class RemoveTranslation
{
    public record Command(Guid TermId, Guid LanguageId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageId).NotEmpty();
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

            term.RemoveTranslation(languageId);

            terms.Update(term);
        }
    }
}