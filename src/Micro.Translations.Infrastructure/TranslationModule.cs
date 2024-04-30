using Micro.Common;
using Micro.Users;

namespace Micro.Translations.Infrastructure;

public interface ITranslationModule : IModule;

public class TranslationModule() : BaseModule(TranslationsCompositionRoot.BeginLifetimeScope), ITranslationModule;