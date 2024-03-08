using Micro.Tenants.Domain.Projects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class ProjectNameConverter() : ValueConverter<ProjectName, string>(v => v.Value, v => new ProjectName(v));