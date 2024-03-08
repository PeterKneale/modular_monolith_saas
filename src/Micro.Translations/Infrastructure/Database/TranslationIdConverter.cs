﻿using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TranslationIdConverter() : ValueConverter<TranslationId, Guid>(v => v.Value, v => TranslationId.Create(v));