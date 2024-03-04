using Micro.Translations.Domain.Translations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TranslationIdConverter() : ValueConverter<TranslationId, Guid>(v => v.Value, v => new TranslationId(v));