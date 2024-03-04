using Micro.Translations.Domain.Terms;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TermNameConverter() : ValueConverter<TermName, string>(v => v.Value, v => new TermName(v));