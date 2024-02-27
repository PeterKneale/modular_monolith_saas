using System.Data;
using Dapper;
using Micro.Tenants.Domain.ApiKeys;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Infrastructure.Dapper;

public class ProjectNameTypeHandler : SqlMapper.TypeHandler<ProjectName>
{
    private ProjectNameTypeHandler()
    {
    }

    public static readonly ProjectNameTypeHandler Default = new();

    public override ProjectName Parse(object? value)
    {
        if (value is string name)
        {
            return new ProjectName(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(ProjectName)}");
    }

    public override void SetValue(IDbDataParameter parameter, ProjectName? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}