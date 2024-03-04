using Micro.Translations.Domain;
using Micro.Translations.Domain.Languages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class LanguageCodeConverter() : ValueConverter<LanguageCode, string>(v => v.Code, v => LanguageCode.FromIsoCode(v));