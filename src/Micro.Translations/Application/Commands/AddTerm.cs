using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application.Commands;

public static class AddTerm
{
    public record Command(Guid TermId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ITermRepository terms, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var termId = new TermId(command.TermId);
            if (await terms.GetAsync(termId, token) != null) throw new AlreadyExistsException(termId);

            var name = new TermName(command.Name);
            if (await terms.GetAsync(projectId, name, token) != null) throw new AlreadyInUseException(name);

            var term = new Term(termId, projectId, name);
            await terms.CreateAsync(term, token);

            return Unit.Value;
        }
    }
}