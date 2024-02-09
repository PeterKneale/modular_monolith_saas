using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application.Languages;

public static class AddLanguage
{
    public record Command(Guid LanguageId, Guid AppId, string LanguageCode) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
            RuleFor(m => m.AppId).NotEmpty();
            RuleFor(m => m.LanguageCode).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(ILanguageRepository languages) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var languageId = new LanguageId(command.LanguageId);
            var appId = new AppId(command.AppId);
            var languageCode = LanguageCode.FromIsoCode(command.LanguageCode);
            var language = new Language(languageId, appId, languageCode);
            
            await languages.CreateAsync(language);
            return Unit.Value;
        }
    }
}