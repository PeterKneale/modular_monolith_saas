using Micro.Common;
using Micro.Translations.Infrastructure;
using Micro.Users;

namespace Micro.Translations;

public interface ITranslationModule : IModule;

public class TranslationModule() : BaseModule(CompositionRoot.BeginLifetimeScope), ITranslationModule;