using Micro.Translations.Domain.Languages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class LanguageIdConverter() : ValueConverter<LanguageId, Guid>(v => v.Value, v => new LanguageId(v));