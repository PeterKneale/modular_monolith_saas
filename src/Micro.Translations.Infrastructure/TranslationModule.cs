using Micro.Common;
using Micro.Users;

namespace Micro.Translations.Infrastructure;

public interface ITranslationModule : IModule;

[ExcludeFromCodeCoverage]
public class TranslationModule() : BaseModule(TranslationsCompositionRoot.BeginLifetimeScope), ITranslationModule;