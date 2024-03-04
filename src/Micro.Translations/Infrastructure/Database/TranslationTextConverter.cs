using Micro.Translations.Domain.Translations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TranslationTextConverter() : ValueConverter<TranslationText, string>(v => v.Value, v => new TranslationText(v));