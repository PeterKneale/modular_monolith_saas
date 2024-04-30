using Micro.Translations.Domain.LanguageAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Infrastructure.Database.Converters;

public class LanguageDetailConverter() : ValueConverter<LanguageDetail, string>(v => v.Code, v => LanguageDetail.Create(v));