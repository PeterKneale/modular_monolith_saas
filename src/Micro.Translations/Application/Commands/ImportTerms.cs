using Micro.Translations.Domain.TermAggregate;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Micro.Translations.Application.Commands;

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
            var names = command.Names
                .Select(TermName.Create)
                .ToList();

            var existing = (await repository.ListAsync(projectId, token)).ToList();

            foreach (var name in names)
            {
                if (existing.SingleOrDefault(x => x.Name.Equals(name)) != null)
                {
                    continue;
                }
                var term = Term.Create(projectId, name);
                await repository.CreateAsync(term, token);
            }

            return Unit.Value;
        }
    }
}