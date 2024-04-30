using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Infrastructure.Database.Converters;

public class TermIdConverter() : ValueConverter<TermId, Guid>(v => v.Value, v => TermId.Create(v));