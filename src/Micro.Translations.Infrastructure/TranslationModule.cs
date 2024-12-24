using Micro.Common;

namespace Micro.Translations.Infrastructure;

public interface ITranslationModule : IModule;

[ExcludeFromCodeCoverage]
public class TranslationModule() : BaseModule(TranslationsCompositionRoot.BeginAsyncLifetimeScope), ITranslationModule;