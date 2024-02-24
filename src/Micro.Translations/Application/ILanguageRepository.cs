﻿using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Application;

public interface ILanguageRepository
{
    Task CreateAsync(Language language, CancellationToken token);
}