using Micro.Common.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Common.Infrastructure.Database;

public class ProjectIdConverter() : ValueConverter<ProjectId, Guid>(v => v.Value, v => new ProjectId(v));