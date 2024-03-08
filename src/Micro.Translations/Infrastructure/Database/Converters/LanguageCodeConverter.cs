using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class LanguageCodeConverter() : ValueConverter<Language, string>(v => v.Code, v => Language.FromIsoCode(v));