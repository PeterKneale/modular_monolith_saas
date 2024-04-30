namespace Micro.Tenants.Infrastructure.Infrastructure.Database.Converters;

public class ProjectNameConverter() : ValueConverter<ProjectName, string>(v => v.Value, v => ProjectName.Create(v));