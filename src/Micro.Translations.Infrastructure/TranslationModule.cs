using Micro.Common;
using Micro.Translations.Infrastructure.Infrastructure;
using Micro.Users;

namespace Micro.Translations.Infrastructure;

public interface ITranslationModule : IModule;

public class TranslationModule() : BaseModule(CompositionRoot.BeginLifetimeScope), ITranslationModule;