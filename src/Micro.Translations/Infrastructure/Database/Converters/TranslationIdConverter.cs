using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database.Converters;

public class TranslationIdConverter() : ValueConverter<TranslationId, Guid>(v => v.Value, v => TranslationId.Create(v));