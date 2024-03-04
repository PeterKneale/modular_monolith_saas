using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Translations.Infrastructure.Database;

public class ProjectIdConverter() : ValueConverter<ProjectId, Guid>(v => v.Value, v => new ProjectId(v));