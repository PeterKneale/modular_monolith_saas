using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application.Terms.Commands;

public static class ImportTerms
{
    private const int MaxItems = 1000;

    public record Command(string[] Names) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Names).NotEmpty().Must(x => x.Length <= MaxItems);
        }
    }

    public class Handler(ITermRepository repository, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var list = command.Names.Select(name => Term.Create(name, projectId)).ToList();
            var existing = await repository.ListAsync(projectId, token);
            var remaining = list.Except(existing);

            foreach (var term in remaining) await repository.CreateAsync(term, token);

            return Unit.Value;
        }
    }
}