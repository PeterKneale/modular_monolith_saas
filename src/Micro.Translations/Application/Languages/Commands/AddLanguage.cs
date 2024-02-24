using Micro.Common.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Application.Languages.Commands;

public static class AddLanguage
{
    public record Command(Guid LanguageId,  string LanguageCode) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
            RuleFor(m => m.LanguageCode).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ILanguageRepository languages, IProjectExecutionContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var languageId = new LanguageId(command.LanguageId);
            var projectId = context.ProjectId;
            var languageCode = LanguageCode.FromIsoCode(command.LanguageCode);
            var language = new Language(languageId, projectId, languageCode);

            await languages.CreateAsync(language, token);
            return Unit.Value;
        }
    }
}