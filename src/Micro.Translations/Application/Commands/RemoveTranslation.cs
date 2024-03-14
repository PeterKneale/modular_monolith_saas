using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

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
        public async Task Handle(Command command, CancellationToken token)
        {
            var termId = TermId.Create(command.TermId);
            var language = Language.FromIsoCode(command.LanguageCode);

            var term = await terms.GetAsync(termId, token);
            if (term == null) throw new NotFoundException(nameof(term), termId.Value);

            term.RemoveTranslation(language);

            terms.Update(term);
            
        }
    }
}