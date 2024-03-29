using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.IntegrationEvents;

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

    public class Handler(ITermRepository terms, IExecutionContext context, OutboxWriter events) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var termId = TermId.Create(command.TermId);
            if (await terms.GetAsync(termId, token) != null) throw new AlreadyExistsException(nameof(Term), termId);

            var name = TermName.Create(command.Name);
            if (await terms.GetAsync(projectId, name, token) != null) throw new AlreadyInUseException(nameof(TermName), name);

            var term = Term.Create(termId, projectId, name);
            await terms.CreateAsync(term, token);

            await events.WriteAsync(new TermChanged(termId, name), token);
        }
    }
}