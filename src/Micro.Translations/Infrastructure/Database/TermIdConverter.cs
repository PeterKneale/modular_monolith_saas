using Micro.Translations.Domain.Terms;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class TermIdConverter() : ValueConverter<TermId, Guid>(v => v.Value, v => new TermId(v));