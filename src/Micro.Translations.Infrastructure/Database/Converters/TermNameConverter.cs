using Micro.Translations.Domain.TermAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database.Converters;

public class TermNameConverter() : ValueConverter<TermName, string>(v => v.Value, v => TermName.Create(v));