using System.Data;
using Dapper;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Infrastructure.Dapper;

public class OrganisationNameTypeHandler : SqlMapper.TypeHandler<OrganisationName>
{
    private OrganisationNameTypeHandler()
    {
    }

    public static readonly OrganisationNameTypeHandler Default = new();

    public override OrganisationName Parse(object? value)
    {
        if (value is string name)
        {
            return new OrganisationName(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(OrganisationName)}");
    }

    public override void SetValue(IDbDataParameter parameter, OrganisationName? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}