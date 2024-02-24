using Micro.Common.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application.Terms.Commands;

public static class AddTerm
{
    public record Command(Guid TermId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITermRepository terms, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var termId = new TermId(command.TermId);
            var projectId = context.ProjectId;
            var name = new TermName(command.Name);
            var term = new Term(termId, projectId, name);
            await terms.CreateAsync(term, token);
            return Unit.Value;
        }
    }
}