using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TranslationTextConverter() : ValueConverter<TranslationText, string>(v => v.Value, v => TranslationText.Create(v));