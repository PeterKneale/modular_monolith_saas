using Micro.Translations.Domain.LanguageAggregate;

namespace Micro.Translations.Application.Commands;

public static class AddLanguage
{
    public record Command(Guid LanguageId, string Code) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
            RuleFor(m => m.Code).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ILanguageRepository languages, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = LanguageId.Create(command.LanguageId);
            var code = command.Code;

            if (await languages.GetAsync(languageId, token) != null)
            {
                throw new AlreadyExistsException(nameof(Language), languageId);
            }

            if (await languages.GetAsync(projectId, code, token) != null)
            {
                throw new AlreadyInUseException(nameof(Language), code);
            }

            var language = Language.FromIsoCode(languageId, projectId, code);
            await languages.CreateAsync(language, token);
        }
    }
}