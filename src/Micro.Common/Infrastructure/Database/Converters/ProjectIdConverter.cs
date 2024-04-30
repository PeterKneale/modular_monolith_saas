using Micro.Common.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Common.Infrastructure.Database.Converters;

public class ProjectIdConverter() : ValueConverter<ProjectId, Guid>(v => v.Value, v => ProjectId.Create(v));